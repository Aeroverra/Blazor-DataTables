using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tech.Aerove.Blazor.DataTables.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Tech.Aerove.Blazor.DataTables.Extensions;
using Tech.Aerove.Blazor.DataTables.Models;

namespace Tech.Aerove.Blazor.DataTables.Configs
{
    /// <summary>
    /// Initial start up config
    /// Used to set defaults and override defaults on a per column basis
    /// </summary>
    public class InitialConfig : ICloneable
    {
        /// <summary>
        /// Whether a column will be orderable by default
        /// </summary>
        public bool DefaultOrderable = false;

        /// <summary>
        /// Whether a column will be filterable by default
        /// </summary>
        public bool DefaultFilterable = false;

        /// <summary>
        /// Whether a column will be searchable by default
        /// </summary>
        public SearchMode DefaultSearchMode = SearchMode.Disabled;

        /// <summary>
        /// The maximum amount of records which can be queried
        /// </summary>
        public int MaxLength = 100;

        /// <summary>
        /// Enables the length dropdown menu
        /// </summary>
        public bool UIEnableLengthDropdown = true;

        /// <summary>
        /// The lengths shown in the top left dropdown menu also known as the draw lengths
        /// </summary>
        public int[] UIDisplayedLengths = new int[] { 10, 25, 50, 100 };

        /// <summary>
        /// Enables the search bar
        /// </summary>
        public bool UIEnableSearchBar = true;

        /// <summary>
        /// The default values or overrides for specified columns.
        /// Values set using the Columns() ColumnBuilder by the user
        /// </summary>
        internal List<ColumnOverrideModel> Columns = new List<ColumnOverrideModel>();

        /// <summary>
        /// This will contain any columns to be used for ordering in the order they will apply
        /// ie: order by name desc then by id asc
        /// This will contain 0 columns if no columns are being used for ordering
        /// </summary>
        internal List<string> ColumnsOrdered = new List<string>();

        /// <summary>
        /// Set the property specific columns
        /// </summary>
        public ColumnBuilder Column<TItem>(Expression<Func<TItem, object>> propertyExpression)
        {
            return new ColumnBuilder(this, propertyExpression.GetPropertyInfo().Name);
        }

        internal InitialConfig() { }

        /// <summary>
        /// Clones this object to maintain values and prevent unexpected behavior
        /// Should not be used past configuration but this is another failsafe
        /// </summary>
        public object Clone()
        {
            return base.MemberwiseClone();
        }
    }
}
