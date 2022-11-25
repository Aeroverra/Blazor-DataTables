using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Tech.Aerove.Blazor.DataTables.Api;
using Tech.Aerove.Blazor.DataTables.Configs;

namespace Tech.Aerove.Blazor.DataTables.Context
{
    public abstract class TableContext<TItem> : ITableContext
    {
        /// <summary>
        /// The central data processing location which handles the setup
        /// calls the overrides and manages the data
        /// </summary>
        internal readonly Engine<TItem> Engine;

        /// <summary>
        /// The api allowing control over table operations
        /// </summary>
        internal readonly ITableController TableController;

        /// <summary>
        /// Configuration specific to the ui
        /// </summary>
        internal readonly UIConfig UIConfig = new UIConfig();

        /// <summary>
        /// Configuration specific to the actual table
        /// </summary>
        internal readonly RunningConfig RunningConfig = new RunningConfig();

        /// <summary>
        /// Used internally to catch 
        /// </summary>
        IEngine ITableContext.Engine => Engine;
        ITableController ITableContext.Api => TableController;
        UIConfig ITableContext.UIConfig => UIConfig;
        RunningConfig ITableContext.RunningConfig => RunningConfig;

        public TableContext()
        {
            Engine = new Engine<TItem>(this);
            TableController = new TableController<TItem>(this);
        }

        /// <summary>
        /// Used to set the initial config and overrides
        /// </summary>
        /// <param name="config"></param>
        protected virtual void OnConfiguring(InitialConfig config)
        {

        }

        /// <summary>
        /// This should return the IQueryable which will be used to query the data
        /// </summary>
        /// <returns></returns>
        protected abstract Task<IQueryable<TItem>> OnStartQueryAsync();

        /// <summary>
        /// An optional way to modify or reinforce constraints before the query is evaluated
        /// </summary>
        protected virtual Task<IQueryable<TItem>> OnBeforeFinishQueryAsync(IQueryable<TItem> query)
        {
            return Task.FromResult(query);
        }

        /// <summary>
        /// Optional way to know when the query finished. Good way to dispose of contexts
        /// </summary>
        protected virtual Task OnAfterQueryAsync(bool success) => Task.CompletedTask;


        /// <summary>
        /// used internally to invoke the protected version
        /// </summary>
        internal Task<IQueryable<TItem>> StartQueryAsync() => OnStartQueryAsync();

        /// <summary>
        /// used internally to invoke the protected version
        /// </summary>
        internal Task<IQueryable<TItem>> BeforeFinishQueryAsync(IQueryable<TItem> query) => OnBeforeFinishQueryAsync(query);

        /// <summary>
        /// used internally to invoke the protected version
        /// </summary>
        internal Task AfterQueryAsync(bool success) => OnAfterQueryAsync(success);

        /// <summary>
        /// used internally to invoke the protected version
        /// </summary>
        internal void Configuring(InitialConfig config) => OnConfiguring(config);

    }
}
