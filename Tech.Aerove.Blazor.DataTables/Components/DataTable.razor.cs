using System.ComponentModel.DataAnnotations;
using Tech.Aerove.Blazor.DataTables.Context;
using Tech.Aerove.Blazor.DataTables.Models;

namespace Tech.Aerove.Blazor.DataTables.Components
{
    /// <summary>
    /// Main Component which handles the initial instantiation of the sub components and shared data
    /// </summary>
    /// <typeparam name="TItem">The Object type to be queried</typeparam>
    public partial class DataTable<TItem> : ComponentBase
    {
        /// <summary>
        /// The Table Head render template. If not specified no table heade will be generated
        /// </summary>
        [Parameter] public RenderFragment? TableHead { get; set; }

        /// <summary>
        /// The Table Body render template. If not specified no table heade will be generated
        /// </summary>
        [Parameter] public RenderFragment<TableBodyTemplateModel<TItem>>? TableBody { get; set; }

        /// <summary>
        /// The Source of the data which will be queries. Defined by the User
        /// </summary>
        [Parameter, Required] public TableContext<TItem> TableContext { get; set; } = null!;

        /// <summary>
        /// Table attributes that will be pasted to the html table element
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object>? InputAttributes { get; set; } = new Dictionary<string, object>();


        protected override Task OnInitializedAsync()
        {
            return TableContext.Engine.UpdateAsync();
        }

    }
}