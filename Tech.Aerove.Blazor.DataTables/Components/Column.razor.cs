using Microsoft.AspNetCore.Components;

namespace Tech.Aerove.Blazor.DataTables.Components
{
    /// <summary>
    /// Used by the user to define a table column
    /// </summary>
    public partial class Column : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? InputAttributes { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }//inner content
    }
}