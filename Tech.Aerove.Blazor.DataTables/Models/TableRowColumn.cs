using System.Reflection;

namespace Tech.Aerove.Blazor.DataTables.Models
{
    public class TableRowColumn<TItem>
    {
        public string Name { get; set; }
        public object? Value { get; set; }
        public TItem Item { get; set; }
        public List<TItem> Items { get; set; }

        public TableRowColumn(List<TItem> items, TItem item, PropertyInfo propertyInfo)
        {
            Items = items.ToList();
            Item = item;
            Name = propertyInfo.Name;
            Value = propertyInfo.GetValue(item);
        }

        public T? GetValue<T>()
        {
            if (Value == null)
            {
                return default;
            }
            return (T)Value;
        }

    }
}
