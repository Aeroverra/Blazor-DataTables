using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Tech.Aerove.Blazor.DataTables.Api;
using Tech.Aerove.Blazor.DataTables.Configs;
using Tech.Aerove.Blazor.DataTables.Context;
using Tech.Aerove.Blazor.DataTables.Enums;
using Tech.Aerove.Blazor.DataTables.Extensions;
using Tech.Aerove.Blazor.DataTables.Models;

namespace Tech.Aerove.Blazor.DataTables.Components
{
    /// <summary>
    /// Used by the user to define a table header
    /// </summary>
    public partial class Header : ComponentBase
    {
        [CascadingParameter] internal ITableContext Context { get; set; } = null!;
        private UIConfig UIConfig => Context.UIConfig;
        private RunningConfig RunningConfig => Context.RunningConfig;
        private IEngine Engine => Context.Engine;
        private TableController Api => Context.TableController;

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> InputAttributes { get; set; } = new Dictionary<string, object>();

        [Parameter]
        public string? Name { get; set; } //User Passed Name to specify which header this should be

        [Parameter]
        public RenderFragment? ChildContent { get; set; }//inner content created by user

        private ColumnModel? ColumnModel;

        protected override void OnParametersSet()
        {
            if (ColumnModel == null && Name != null)
            {
                ColumnModel = Api.GetColumn(Name);
            }
            if (ColumnModel != null && ColumnModel.OrderableDirection != OrderableDirection.Disabled)
            {
                InputAttributes.RemoveStyleClass(OrderableDirection.Ascending.GetClass());
                InputAttributes.RemoveStyleClass(OrderableDirection.Ascending.GetClass());
                InputAttributes.AddStyleClass("orderable");
                InputAttributes.AddStyleClass(ColumnModel.OrderableDirection.GetClass());
            }

        }
        private async Task ChangeDirectionAsync(MouseEventArgs args)
        {
            Api.SwapOrder(ColumnModel!, args.ShiftKey);
            await Api.UpdateAsync();
        }
    }
}