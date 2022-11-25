﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
    public class InitialConfig : ICloneable
    {
        /// <summary>
        /// Whether a column will be orderable by default
        /// </summary>
        public bool DefaultOrderable = false;

        /// <summary>
        /// Whether a column will be searchable by default
        /// </summary>
        public SearchMode DefaultSearchMode = SearchMode.Disabled;

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
