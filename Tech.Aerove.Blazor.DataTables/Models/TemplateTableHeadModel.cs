using System.Reflection;
using Tech.Aerove.Blazor.DataTables.Attributes;
using Tech.Aerove.Blazor.DataTables.Components;

namespace Tech.Aerove.Blazor.DataTables.Models
{

    /// <summary>
    /// The Model passed when making a header template 
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class TemplateTableHeadModel<TItem>
    {
        public string Name { get; set; }
        public List<TItem> Items { get; set; }

        public TemplateTableHeadModel(List<TItem> items, PropertyInfo propertyInfo)
        {
            Items = items.ToList();
            Name = propertyInfo.Name;
        }

    }
}
