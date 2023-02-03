using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Aerove.Blazor.DataTables.Configs;
using Tech.Aerove.Blazor.DataTables.Enums;
using Tech.Aerove.Blazor.DataTables.Models;

namespace Tech.Aerove.Blazor.DataTables.Context
{
    /// <summary>
    /// The central data processing location which handles the setup
    /// calls the overrides and manages the data
    /// </summary>
    internal class Engine<TItem> : IEngine, IDisposable
    {
        private readonly TableContext<TItem> Context;
        private UIConfig UIConfig => Context.UIConfig;
        private RunningConfig RunningConfig => Context.RunningConfig;

        /// <summary>
        /// The raw queried data
        /// </summary>
        internal List<TItem> Items = new List<TItem>();

        public Engine(TableContext<TItem> tableContext)
        {
            Context = tableContext;
            //call configure to set our defaults and overrides
            Configure();
        }

        private SemaphoreSlim QueryLock = new SemaphoreSlim(1);

        /// <summary>
        /// Updates the Items 
        /// </summary>
        public async Task UpdateAsync()
        {
            await QueryLock.WaitAsync();
            try
            {
                var query = await Context.StartQueryAsync();

                //total records available from source
                RunningConfig.RecordsTotal = await query.CountAsync();

                //applies search text
                query = query.Search(RunningConfig.Columns, RunningConfig.SearchText);

                //applies filters
                query = query.Filter(RunningConfig.Columns);

                //allows user to use more advanced filtering
                query = await Context.FilterQueryAsync(query);

                //total records after being filtered
                RunningConfig.RecordsFiltered = await query.CountAsync();

                if(RunningConfig.ColumnsOrdered.Count > 0)
                {
                    //sets order
                    query = query.Order(RunningConfig.ColumnsOrdered);
                }
                else
                {
                    //allow user to use more advanced ordering
                    query = await Context.OrderQueryAsync(query);
                }

                //skip records based on the current page
                query = query.Skip((RunningConfig.Page - 1) * RunningConfig.Length);

                //only take a specific set of records
                query = query.Take(RunningConfig.Length);

                //allow user to add or modify constraints before query is evaluated
                query = await Context.BeforeFinishQueryAsync(query);

                //finish the query and store the result
                Items = await query.FinishQueryAsync();

                //allow user to perform actions after the query is complete like
                //disposing a db context
                await Context.AfterQueryAsync(true);

            }
            finally
            {
                QueryLock.Release();
            }
        }


        /// <summary>
        /// Called from the constructor and sets up the running and ui configs
        /// </summary>
        private void Configure()
        {
            var initialConfig = new InitialConfig();

            //Let the user configure defaults and overrides
            Context.Configuring(initialConfig);

            //Get a clone to prevent user from modifying it anymore
            initialConfig = (InitialConfig)initialConfig.Clone();

            //Sets the default values based on the users prefrences
            UIConfig.Setup(initialConfig);

            //set the default legnth
            RunningConfig.Length = initialConfig.Length;

            //set the max length constraint
            RunningConfig.MaxLength = initialConfig.MaxLength;

            //get columns from properties and sets the default values based on the users prefrences
            var properties = typeof(TItem).GetProperties();
            foreach (var property in properties)
            {
                var column = new ColumnModel(property.Name)
                {
                    OrderableDirection = initialConfig.DefaultOrderable == false ? OrderableDirection.Disabled : OrderableDirection.None,
                    SearchMode = initialConfig.DefaultSearchMode,
                    Filterable = initialConfig.DefaultFilterable
                };
                RunningConfig.Columns.Add(column);
            }

            //Get columns from fields and sets the default values based on the users prefrences
            var fields = typeof(TItem).GetFields();
            foreach (var field in fields)
            {
                var column = new ColumnModel(field.Name)
                {
                    OrderableDirection = initialConfig.DefaultOrderable == false ? OrderableDirection.Disabled : OrderableDirection.None,
                    SearchMode = initialConfig.DefaultSearchMode,
                    Filterable = initialConfig.DefaultFilterable
                };
                RunningConfig.Columns.Add(column);
            }


            //set values from any user overrides
            foreach (var columnOverride in initialConfig.Columns)
            {
                var column = RunningConfig.Columns.Single(x => x.Name == columnOverride.Name);
                columnOverride.OverrideColumn(column);
            }

            //set the order in which the orderables should be applied
            foreach(var columnOrder in initialConfig.ColumnsOrdered)
            {
                var column = RunningConfig.Columns.Single(x => x.Name == columnOrder);
                RunningConfig.ColumnsOrdered.Add(column);
            }

        }

        public void Dispose()
        {
            QueryLock.Dispose();
        }
    }
}
