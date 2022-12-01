using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Aerove.Blazor.DataTables.Enums;

namespace Tech.Aerove.Blazor.DataTables.Models
{
    /// <summary>
    /// Holds the initial configuration overides for a column
    /// Not used after initial configuration. The difference between this and the 
    /// ColumnModel is the functions and nullability.
    /// </summary>
    internal class ColumnOverrideModel
    {
        internal readonly string Name;
        internal OrderableDirection? OrderableDirection { get; set; }
        internal SearchMode? SearchMode { get; set; }
        internal bool? Filterable = false;
        internal List<string>? Filters;

        internal ColumnOverrideModel(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Overrides the default values of columns
        /// </summary>
        internal void OverrideColumn(ColumnModel column)
        {
            if (OrderableDirection != null)
            {
                column.OrderableDirection = OrderableDirection.Value;
            }
            if(SearchMode != null)
            {
                column.SearchMode = SearchMode.Value;
            }
            if(Filterable!= null)
            {
                column.Filterable = Filterable.Value;
            }
            if(Filters != null)
            {
                column.Filters = Filters;
            }

        }
    }
}
