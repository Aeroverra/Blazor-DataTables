using System.ComponentModel.DataAnnotations;
using Tech.Aerove.Blazor.DataTables.Context;
using Tech.Aerove.Blazor.DataTables.Models;

namespace Tech.Aerove.Blazor.DataTables.Components
{
    public partial class Table<TItem> : ComponentBase
    {
        [CascadingParameter] internal TableContext<TItem> Context { get; set; } = null!;
        private UIConfig UIConfig => Context.UIConfig;
        private RunningConfig RunningConfig => Context.RunningConfig;
        private Engine<TItem> Engine => Context.Engine;


        /// <summary>
        /// The Table Head render template. If not specified no table header will be generated. 
        /// Passed down from the DataTable Component
        /// </summary>
        [Parameter] public RenderFragment? TableHead { get; set; }

        /// <summary>
        /// The Table Body render template. If not specified no table heade will be generated
        /// Passed down from the DataTable Component
        /// </summary>
        [Parameter] public RenderFragment<TableBodyTemplateModel<TItem>>? TableBody { get; set; }

        /// <summary>
        /// Table attributes that will be pasted to the html table element
        /// </summary>
        [Parameter, Required] public Dictionary<string, object>? InputAttributes { get; set; } = new Dictionary<string, object>();



    }
}