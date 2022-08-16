using System.Reflection;

namespace Tech.Aerove.Blazor.DataTables.Models
{
    public class TableHeadColumn<TItem>
    {
        public string Name { get; set; }
        public List<TItem> Items { get; set; }

        public TableHeadColumn(List<TItem> items, PropertyInfo propertyInfo)
        {
            Items = items.ToList();
            Name = propertyInfo.Name;
        }


    }
}
