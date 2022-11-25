using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Tech.Aerove.Blazor.DataTables.Enums;
using Tech.Aerove.Blazor.DataTables.Models;

namespace Tech.Aerove.Blazor.DataTables.Configs
{
    public class ColumnBuilder
    {
        private readonly InitialConfig Config;
        private readonly string Name;
        private readonly ColumnOverrideModel Column;
        internal ColumnBuilder(InitialConfig config, string name)
        {
            Config = config;
            Name = name;

            //Gets the column override model and creates one if it doesn't already exist
            var column = Config.Columns.SingleOrDefault(x => x.Name == Name);
            if (column == null)
            {
                column = new ColumnOverrideModel(Name);
                Config.Columns.Add(column);
            }
            Column = column;
        }

        /// <summary>
        /// Sets the default orderable direction
        /// To turn on without a default set it to None
        /// To turn off set it to Disabled
        /// </summary>
        public ColumnBuilder Order(OrderableDirection? orderable)
        {
            Column.OrderableDirection = orderable;
            return this;
        }

        /// <summary>
        /// Sets which search mode to use
        /// Set turn search off set to disable
        /// </summary>
        public ColumnBuilder SearchMode(SearchMode? searchMode)
        {
            Column.SearchMode = searchMode;
            return this;
        }

        /// <summary>
        /// Whether to allow filtering
        /// </summary>
        public ColumnBuilder Filterable(bool? filterable)
        {
            Column.Filterable = filterable;
            return this;
        }

        /// <summary>
        /// Sets the default applied filters
        /// </summary>
        public ColumnBuilder Filters(List<string> filters)
        {
            Column.Filters = filters;
            return this;
        }



    }
}
