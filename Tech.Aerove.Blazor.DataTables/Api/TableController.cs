using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Aerove.Blazor.DataTables.Configs;
using Tech.Aerove.Blazor.DataTables.Context;

namespace Tech.Aerove.Blazor.DataTables.Api
{
    public class TableController<TItem> : ITableController
    {
        private readonly TableContext<TItem> Context;
        private UIConfig UIConfig => Context.UIConfig;
        private RunningConfig RunningConfig => Context.RunningConfig;

        public TableController(TableContext<TItem> tableContext)
        {
            Context = tableContext;
        }
    }
}
