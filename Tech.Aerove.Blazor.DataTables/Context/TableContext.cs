using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Tech.Aerove.Blazor.DataTables.Api;
using Tech.Aerove.Blazor.DataTables.Configs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Tech.Aerove.Blazor.DataTables.Context
{
    public abstract class TableContext<TItem> : ITableContext, IDisposable
    {
        /// <summary>
        /// The central data processing location which handles the setup
        /// calls the overrides and manages the data
        /// </summary>
        internal readonly Engine<TItem> Engine;

        /// <summary>
        /// The api allowing control over table operations
        /// </summary>
        public readonly TableController Api;

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
        TableController ITableContext.TableController => Api;
        UIConfig ITableContext.UIConfig => UIConfig;
        RunningConfig ITableContext.RunningConfig => RunningConfig;

        public TableContext()
        {
            Engine = new Engine<TItem>(this);
            Api = new TableController(this);
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
        /// An optional way to apply more advanced filters
        /// </summary>
        /// <returns></returns>
        protected virtual Task<IQueryable<TItem>> OnFilterQueryAsync(IQueryable<TItem> query)
        {
            return Task.FromResult(query);
        }

        /// <summary>
        /// An optional way to apply more advanced ordering
        /// </summary>
        /// <returns></returns>
        protected virtual Task<IQueryable<TItem>> OnOrderQueryAsync(IQueryable<TItem> query)
        {
            return Task.FromResult(query);
        }

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
        internal Task<IQueryable<TItem>> FilterQueryAsync(IQueryable<TItem> query) => OnFilterQueryAsync(query);

        /// <summary>
        /// used internally to invoke the protected version
        /// </summary>
        internal Task<IQueryable<TItem>> OrderQueryAsync(IQueryable<TItem> query) => OnOrderQueryAsync(query);

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


        #region Dispose Pattern

        // To detect redundant calls
        private bool DisposedValue;

        ~TableContext() => Dispose(false);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!DisposedValue)
            {
                if (disposing)
                {
                    Engine.Dispose();
                }
                DisposedValue = true;
            }
        }

        #endregion
    }
}
