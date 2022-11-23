using System.Reflection;
using Tech.Aerove.Blazor.DataTables.Attributes;
using Tech.Aerove.Blazor.DataTables.Models.Enums;

namespace Tech.Aerove.Blazor.DataTables.Models
{

    /// <summary>
    /// Passed as a cascading parameter to the Column Component
    /// </summary>
    internal class ColumnInfoModel
    {
        public PropertyInfo PropertyInfo { get; set; }
        public Type ModelType { get; set; }
        public Type Type { get; set; }
        public string Name { get; private set; }
        public bool Render { get; set; }
        public bool Orderable { get; set; }
        public bool Filterable { get; set; }
        public SearchMode SearchMode { get; set; }

        /// <summary>
        /// Created by the static GetColumns method which makes all the columns at the same time
        /// </summary>
        private ColumnInfoModel(PropertyInfo propertyInfo, Type type)
        {
            PropertyInfo = propertyInfo;
            ModelType = type;
            Type = propertyInfo.PropertyType;
            Name = propertyInfo.Name;
            Render = GetRender();
            Orderable = GetOrderable();
            SearchMode = GetSearchMode();
            Filterable = GetFilterable();
        }

        /// <summary>
        /// This uses reflection to determine all the columns in the given model and whether they
        /// should be searchable, orderable, filterable, renderable etc
        /// </summary>
        public static List<ColumnInfoModel> GetColumns<TItem>()
        {
            var type = typeof(TItem);
            List<PropertyInfo> properties = type.GetProperties().ToList();
            List<ColumnInfoModel> columns = new List<ColumnInfoModel>();

            foreach (var column in properties)
            {
                columns.Add(new ColumnInfoModel(column, type));
            }
            return columns;
        }

        /// <summary>
        /// Determines the search mode of the current column
        /// </summary>
        private SearchMode GetSearchMode()
        {
            var searchable = SearchMode.None;
            var dataTable = ModelType.GetCustomAttribute<DataSearchAttribute>();
            if (dataTable != null)
            {
                searchable = dataTable.Mode;
            }

            var columnAttribute = PropertyInfo.GetCustomAttribute<DataSearchAttribute>();
            if (columnAttribute != null)
            {
                searchable = columnAttribute.Mode;
            }
            return searchable;
        }

        /// <summary>
        /// Determines if the current column is orderable
        /// </summary>
        private bool GetOrderable()
        {
            var orderable = false;
            var dataTable = ModelType.GetCustomAttribute<DataOrderableAttribute>();
            if (dataTable != null)
            {
                orderable = dataTable.Orderable;
            }

            var columnAttribute = PropertyInfo.GetCustomAttribute<DataOrderableAttribute>();
            if (columnAttribute != null)
            {
                orderable = columnAttribute.Orderable;
            }
            return orderable;
        }

        /// <summary>
        /// Determines if the current column should be rendered
        /// </summary>
        private bool GetRender()
        {
            var renderable = true;
            var dataTable = ModelType.GetCustomAttribute<DataRenderAttribute>();
            if (dataTable != null)
            {
                renderable = dataTable.Render;
            }

            var columnAttribute = PropertyInfo.GetCustomAttribute<DataRenderAttribute>();
            if (columnAttribute != null)
            {
                renderable = columnAttribute.Render;
            }
            return renderable;
        }

        /// <summary>
        /// Determines if the current column is filterable
        /// </summary>
        private bool GetFilterable()
        {
            var filterable = false;
            var dataTable = ModelType.GetCustomAttribute<DataFilterableAttribute>();
            if (dataTable != null)
            {
                filterable = dataTable.Filterable;
            }

            var columnAttribute = PropertyInfo.GetCustomAttribute<DataFilterableAttribute>();
            if (columnAttribute != null)
            {
                filterable = columnAttribute.Filterable;
            }
            return filterable;
        }

    }
    internal static class ColumnInfoModelExtensions
    {
        /// <summary>
        /// Checks if any column is orderable
        /// </summary>
        internal static bool Orderable(this List<ColumnInfoModel> columns)
        {
            return columns.Any(x => x.Orderable);
        }

        /// <summary>
        /// Checks if any column is searchable
        /// </summary>
        internal static bool Searchable(this List<ColumnInfoModel> columns)
        {
            return columns.Any(x => x.SearchMode != SearchMode.None);
        }

        /// <summary>
        /// Checks if any column is filterable
        /// </summary>
        internal static bool Filterable(this List<ColumnInfoModel> columns)
        {
            return columns.Any(x => x.Filterable);
        }
    }
}
