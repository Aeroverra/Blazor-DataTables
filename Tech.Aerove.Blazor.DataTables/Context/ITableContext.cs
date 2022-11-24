using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech.Aerove.Blazor.DataTables.Context
{
    internal interface ITableContext
    {
        internal IEngine Engine { get; }
        internal UIConfig UIConfig { get; }
        internal RunningConfig RunningConfig { get; }
    }
}
