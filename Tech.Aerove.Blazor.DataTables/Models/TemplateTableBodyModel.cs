using System.Reflection;

namespace Tech.Aerove.Blazor.DataTables.Models
{
    /// <summary>
    /// The model passed when making a Column template
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class TemplateTableBodyModel<TItem>
    {
        public string Name { get; set; }
        public object? Value { get; set; }
        public TItem Item { get; set; }
        public List<TItem> Items { get; set; }

        public TemplateTableBodyModel(List<TItem> items, TItem item, PropertyInfo propertyInfo)
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
