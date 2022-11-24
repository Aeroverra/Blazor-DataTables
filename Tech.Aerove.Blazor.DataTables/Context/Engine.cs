using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Aerove.Blazor.DataTables.Enums;
using Tech.Aerove.Blazor.DataTables.Models;

namespace Tech.Aerove.Blazor.DataTables.Context
{

    internal class Engine<TItem> : IEngine
    {
        private readonly TableContext<TItem> Context;
        private UIConfig UIConfig => Context.UIConfig;
        private RunningConfig RunningConfig => Context.RunningConfig;

        public event Func<Task> OnAfterUpdate = null!;
        internal List<TItem> Items = new List<TItem>();

        public Engine(TableContext<TItem> tableContext)
        {
            Context = tableContext;
            Configure();
        }

        private SemaphoreSlim QueryLock = new SemaphoreSlim(1);
        public async Task UpdateAsync()
        {
            await QueryLock.WaitAsync();
            try
            {
                var query = await Context.StartQueryAsync();

                RunningConfig.RecordsTotal = await query.CountAsync();

                query = query.Search(RunningConfig.Columns, RunningConfig.SearchText);

                //todo: records filters

                RunningConfig.RecordsFiltered = await query.CountAsync();

                query = query.Order(RunningConfig.ColumnsOrdered);

                query = query.Skip((RunningConfig.Page - 1) * RunningConfig.Length);

                query = query.Take(RunningConfig.Length);


                query = await Context.BeforeFinishQueryAsync(query);

                Items = await query.FinishQueryAsync();

                await Context.AfterQueryAsync(true);

                await OnAfterUpdate();

            }
            finally
            {
                QueryLock.Release();
            }
        }

        private void Configure()
        {
            var properties = typeof(TItem).GetProperties();
            foreach (var property in properties)
            {
                var column = new ColumnModel(property.Name)
                {
                    OrderableDirection = OrderableDirection.None
                };
                RunningConfig.Columns.Add(column);
            }

            var fields = typeof(TItem).GetFields();
            foreach (var field in fields)
            {
                var column = new ColumnModel(field.Name)
                {
                    OrderableDirection = OrderableDirection.None
                };
                RunningConfig.Columns.Add(column);
            }
        }


    }
}
