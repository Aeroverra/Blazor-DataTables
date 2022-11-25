using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Aerove.Blazor.DataTables.Enums;

namespace Tech.Aerove.Blazor.DataTables.Models
{
    /// <summary>
    /// Holds the running configuration for a table column 
    /// </summary>
    internal class ColumnModel
    {
        internal readonly string Name;
        internal OrderableDirection OrderableDirection = OrderableDirection.Disabled;
        internal SearchMode SearchMode = SearchMode.Disabled;
        internal bool Filterable = false;
        internal List<string> Filters = new List<string>();

        public ColumnModel(string name)
        {
            Name = name;
        }
    }
}
