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
        internal int Page { get; set; } = 1;
        internal int RecordsTotal { get; set; } = 0;
        internal int RecordsFiltered { get; set; } = 0;
        internal int Start { get; set; } = 0;
        internal int Length { get; set; } = 10;
        internal int TotalPages => (RecordsFiltered / Length + (RecordsFiltered % Length > 0 ? 1 : 0));
        internal string SearchText = string.Empty;

        internal readonly List<ColumnModel> Columns = new List<ColumnModel>();

        /// <summary>
        /// This will contain any columns to be used for ordering in the order they will apply
        /// ie: order by name desc then by id asc
        /// They will be the same objects in the Columns field.
        /// This will contain 0 columns if no columns are being used for ordering
        /// </summary>
        internal readonly List<ColumnModel> ColumnsOrdered = new List<ColumnModel>();
    }
}
