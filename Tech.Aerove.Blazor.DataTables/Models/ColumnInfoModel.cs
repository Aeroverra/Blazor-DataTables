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
        public TableData TableData { get; set; }
        public string Name { get; private set; }
        public bool Render { get; set; }
        public bool Orderable { get; set; }
        public bool Filterable { get; set; }
        public SearchMode SearchMode { get; set; }

        public ColumnInfoModel(TableData tableData, PropertyInfo propertyInfo, Type type)
        {
            PropertyInfo = propertyInfo;
            ModelType = type;
            Type = propertyInfo.PropertyType;
            TableData = tableData;
            Name = propertyInfo.Name;
            Render = GetRender();
            Orderable = GetOrderable();
            SearchMode = GetSearchMode();
            Filterable = GetFilterable();
        }

        public static List<ColumnInfoModel> GetColumns<TItem>(TableData tableData)
        {
            var type = typeof(TItem);
            List<PropertyInfo> properties = type.GetProperties().ToList();
            List<ColumnInfoModel> columns = new List<ColumnInfoModel>();

            foreach (var column in properties)
            {
                columns.Add(new ColumnInfoModel(tableData, column, type));
            }
            return columns;
        }

        private SearchMode GetSearchMode()
        {
            var searchable = SearchMode.None;
            var dataTable = ModelType.GetCustomAttribute<DataSearchAttribute>();
            if (dataTable != null)
            {
                searchable = dataTable.Mode;
            }

            var columnAttribute = PropertyInfo.GetCustomAttribute<DataSearchAttribute>();
            if (columnAttribute != null && columnAttribute.Mode != null)
            {
                searchable = columnAttribute.Mode;
            }
            return searchable;
        }

        private bool GetOrderable()
        {
            var orderable = false;
            var dataTable = ModelType.GetCustomAttribute<DataOrderableAttribute>();
            if (dataTable != null)
            {
                orderable = dataTable.Orderable;
            }

            var columnAttribute = PropertyInfo.GetCustomAttribute<DataOrderableAttribute>();
            if (columnAttribute != null && columnAttribute.Orderable != null)
            {
                orderable = columnAttribute.Orderable;
            }
            return orderable;
        }

        private bool GetRender()
        {
            var renderable = true;
            var dataTable = ModelType.GetCustomAttribute<DataRenderAttribute>();
            if (dataTable != null)
            {
                renderable = dataTable.Render;
            }

            var columnAttribute = PropertyInfo.GetCustomAttribute<DataRenderAttribute>();
            if (columnAttribute != null && columnAttribute.Render != null)
            {
                renderable = columnAttribute.Render;
            }
            return renderable;
        }

        private bool GetFilterable()
        {
            var filterable = false;
            var dataTable = ModelType.GetCustomAttribute<DataFilterableAttribute>();
            if (dataTable != null)
            {
                filterable = dataTable.Filterable;
            }

            var columnAttribute = PropertyInfo.GetCustomAttribute<DataFilterableAttribute>();
            if (columnAttribute != null && columnAttribute.Filterable != null)
            {
                filterable = columnAttribute.Filterable;
            }
            return filterable;
        }

    }
    internal static class ColumnInfoModelExtensions
    {
        internal static bool Orderable(this List<ColumnInfoModel> columns)
        {
            return columns.Any(x => x.Orderable);
        }

        internal static bool Searchable(this List<ColumnInfoModel> columns)
        {
            return columns.Any(x => x.SearchMode != SearchMode.None);
        }

        internal static bool Filterable(this List<ColumnInfoModel> columns)
        {
            return columns.Any(x => x.Filterable);
        }
    }
}
