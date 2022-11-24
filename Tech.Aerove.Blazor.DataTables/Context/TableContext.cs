using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Tech.Aerove.Blazor.DataTables.Context
{
    public abstract class TableContext<TItem> : ITableContext
    {
        internal readonly Engine<TItem> Engine;
        internal readonly UIConfig UIConfig;
        internal readonly RunningConfig RunningConfig;

        IEngine ITableContext.Engine => Engine;
        UIConfig ITableContext.UIConfig => UIConfig;
        RunningConfig ITableContext.RunningConfig => RunningConfig;

        public TableContext()
        {
            Engine = new Engine<TItem>(this);
            UIConfig = new UIConfig();
            RunningConfig = new RunningConfig();
        }

        protected abstract Task<IQueryable<TItem>> OnStartQueryAsync();
        protected abstract Task<IQueryable<TItem>> OnBeforeFinishQueryAsync(IQueryable<TItem> query);
        protected virtual Task OnAfterQueryAsync(bool success) => Task.CompletedTask;

        internal Task<IQueryable<TItem>> StartQueryAsync() => OnStartQueryAsync();
        internal Task<IQueryable<TItem>> BeforeFinishQueryAsync(IQueryable<TItem> query) => OnBeforeFinishQueryAsync(query);
        internal Task AfterQueryAsync(bool success) => OnAfterQueryAsync(success);

    }
}
