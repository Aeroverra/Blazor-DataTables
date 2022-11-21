using Microsoft.AspNetCore.Components;

namespace Tech.Aerove.Blazor.DataTables.Components
{
    public partial class TableBottom<TItem> : ComponentBase
    {
        /// <summary>
        /// Central communication Between all components
        /// Passed down from the DataTable Component
        /// </summary>
        [CascadingParameter] private TableNetwork<TItem> Network { get; set; } = null!;
    }
}