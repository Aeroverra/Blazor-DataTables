using Microsoft.AspNetCore.Components;
using Tech.Aerove.Blazor.DataTables.AOriginalDT.Models;
namespace Tech.Aerove.Blazor.DataTables.AOriginalDT.Components
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