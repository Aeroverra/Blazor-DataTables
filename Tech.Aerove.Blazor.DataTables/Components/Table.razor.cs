using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace Tech.Aerove.Blazor.DataTables.Components
{
    public partial class Table<TItem>: ComponentBase
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
        [Parameter] public RenderFragment<TemplateTableHeadModel<TItem>>? TableHead { get; set; }

        /// <summary>
        /// The Table Body render template. If not specified no table heade will be generated
        /// Passed down from the DataTable Component
        /// </summary>
        [Parameter] public RenderFragment<TemplateTableBodyModel<TItem>>? TableBody { get; set; }



    }
}