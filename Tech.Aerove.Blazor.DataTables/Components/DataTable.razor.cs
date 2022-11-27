using System.ComponentModel.DataAnnotations;
using Tech.Aerove.Blazor.DataTables.Context;
using Tech.Aerove.Blazor.DataTables.Models;

namespace Tech.Aerove.Blazor.DataTables.Components
{
    /// <summary>
    /// Main Component which handles the initial instantiation of the sub components and shared data
    /// </summary>
    /// <typeparam name="TItem">The Object type to be queried</typeparam>
    public partial class DataTable<TItem> : ComponentBase, IDisposable
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
            TableContext.Api.OnAfterUpdate += OnAfterUpdateAsync;
            return TableContext.Api.UpdateAsync();
        }

        /// <summary>
        /// Called after the table data has been updated
        /// </summary>
        public async Task OnAfterUpdateAsync()
        {
            await InvokeAsync(() => StateHasChanged());
        }

        //todo: investigate if the first update should be run in onafterrender or oninitialized
        //init causes slow page responsiveness if slow but onafter render causes an awful flash if fast
        //maybe a loader displayed on long page load would fix this in onafter
        //protected override Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        return TableContext.Api.UpdateAsync();
        //    }
        //    return Task.CompletedTask;
        //}

        public void Dispose()
        {
            TableContext.Api.OnAfterUpdate -= OnAfterUpdateAsync;
        }

    }
}