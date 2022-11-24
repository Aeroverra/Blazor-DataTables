using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Aerove.Blazor.DataTables.Models;

namespace Tech.Aerove.Blazor.DataTables.Context
{
    internal class RunningConfig
    {
        internal int Length { get; set; } = 10;
        internal string SearchText = string.Empty;

        internal readonly List<ColumnModel> Columns = new List<ColumnModel>();
    }
}
