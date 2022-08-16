using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
namespace Tech.Aerove.Blazor.DataTables.Components
{
    public partial class Header
    {
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