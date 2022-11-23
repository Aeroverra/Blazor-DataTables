using System.Reflection;

namespace Tech.Aerove.Blazor.DataTables.Models
{
    /// <summary>
    /// The model passed when making a Column template
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class TemplateTableBodyModel<TItem>
    {
        public TItem Item { get; set; }
        public List<TItem> Items { get; set; }

        public TemplateTableBodyModel(List<TItem> items, TItem item)
        {
            Items = items.ToList();
            Item = item;
        }
    }
}
