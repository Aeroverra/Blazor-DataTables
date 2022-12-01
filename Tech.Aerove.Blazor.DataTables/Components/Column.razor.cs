using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

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