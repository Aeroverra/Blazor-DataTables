using System.Linq.Dynamic.Core;

namespace Tech.Aerove.Blazor.DataTables.Models
{
    internal class FilterModel
    {
        public string ColumnName { get; private set; }
        public List<string> Values { get; private set; }

        public FilterModel(string columnName, List<string> values)
        {
            ColumnName = columnName;
            Values = values;
        }
    }
    internal static class FilterModelExtensions
    {
        internal static void SetFilter(this List<FilterModel> filters, string name, object value)
        {
            var filter = filters.SingleOrDefault(x => x.ColumnName.ToLower() == name.ToLower());
            if (filter == null)
            {
                throw new Exception("Filter not found. Check to make sure you are allowing filtering " +
                    "on this column and that you are passing the right name.");
            }

            filter.Values.Clear();
            if (value == null)
            {
                return;
            }
            if (value.GetType() == typeof(string))
            {
                if (string.IsNullOrEmpty($"{value}"))
                {
                    return;
                }
                filter.Values.Add($"{value}");
                return;
            }
            if (value.GetType() == typeof(string[]))
            {
                var valueList = ((string[])value)
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();

                filter.Values.AddRange(valueList);
                return;
            }

            throw new Exception($"Object of type {value.GetType()} for field {name} not supported for filtering");
        }

        internal static void PopulateFilterList(this List<FilterModel> filters, List<ColumnInfoModel> columnInfo)
        {
            if (filters.Count() != 0)
            {
                return;
            }
            foreach (var info in columnInfo)
            {
                if (info.Filterable)
                {
                    filters.Add(new FilterModel(info.Name, new List<string>()));
                }
            }
        }

        internal static IQueryable AddToQuery(this List<FilterModel> filters, IQueryable query)
        {
            //WARNING: This uses params to prevent SQL Injection!
            foreach (var filter in filters.Where(x => x.Values.Any()))
            {
                List<string> searchStrings = new List<string>();
                for (var x = 0; x < filter.Values.Count(); x++)
                {
                    searchStrings.Add($"{filter.ColumnName} == @{x}");
                }
                query = query.Where(string.Join(" || ", searchStrings), filter.Values.ToArray());

            }
            return query;
        }
    }
}