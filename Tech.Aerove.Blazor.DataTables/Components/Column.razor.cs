using Microsoft.AspNetCore.Components;

namespace Tech.Aerove.Blazor.DataTables.Components
{
    /// <summary>
    /// Used by the user to define a table column
    /// </summary>
    public partial class Column : ComponentBase
    {
        /// <summary>
        /// Central communication Between all components
        /// Passed down from the DataTable Component
        /// </summary>
        [CascadingParameter] private TableNetwork Network { get; set; } = null!;

        [CascadingParameter]
        private string ColumnName { get; set; } = null!; //name of the column this is rendering

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? InputAttributes { get; set; }

        [Parameter]
        public string? Name { get; set; } //Pass the name 

        [Parameter]
        public RenderFragment? ChildContent { get; set; }//inner content
    }
}