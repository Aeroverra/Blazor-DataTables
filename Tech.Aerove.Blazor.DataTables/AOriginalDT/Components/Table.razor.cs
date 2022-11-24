using Microsoft.AspNetCore.Components;
using Tech.Aerove.Blazor.DataTables.AOriginalDT.Models;

namespace Tech.Aerove.Blazor.DataTables.AOriginalDT.Components
{
    public partial class Table<TItem> : ComponentBase
    {
        /// <summary>
        /// Central communication Between all components
        /// Passed down from the DataTable Component
        /// </summary>
        [CascadingParameter] private TableNetwork<TItem> Network { get; set; } = null!;

        /// <summary>
        /// The Table Head render template. If not specified no table header will be generated. 
        /// Passed down from the DataTable Component
        /// </summary>
        [Parameter] public RenderFragment? TableHead { get; set; }

        /// <summary>
        /// The Table Body render template. If not specified no table heade will be generated
        /// Passed down from the DataTable Component
        /// </summary>
        [Parameter] public RenderFragment<TemplateTableBodyModel<TItem>>? TableBody { get; set; }



    }
}