namespace Tech.Aerove.Blazor.DataTables.Models
{
    /// <summary>
    /// The model passed when making a Column template
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class TableBodyTemplateModel<TItem>
    {
        public TItem Item { get; set; }
        public List<TItem> Items { get; set; }

        public TableBodyTemplateModel(List<TItem> items, TItem item)
        {
            Items = items.ToList();
            Item = item;
        }
    }
}
