using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Aerove.Blazor.DataTables.Enums;

namespace Tech.Aerove.Blazor.DataTables.Models
{
    internal class ColumnModel
    {
        internal readonly string Name;
        internal OrderableDirection OrderableDirection = OrderableDirection.Disabled;
        internal SearchMode SearchMode = SearchMode.Disabled;

        public ColumnModel(string name)
        {
            Name = name;
        }
    }
}
