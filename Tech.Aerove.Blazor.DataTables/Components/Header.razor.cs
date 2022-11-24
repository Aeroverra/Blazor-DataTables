using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> InputAttributes { get; set; } = new Dictionary<string, object>();

        [Parameter]
        public string? Name { get; set; } //User Passed Name to specify which header this should be

        [Parameter]
        public RenderFragment? ChildContent { get; set; }//inner content created by user

        private ColumnModel? ColumnModel;

        protected override void OnParametersSet()
        {
            //Random rand = new Random();
            //Thread.Sleep(rand.Next(1000, 5000));
            Console.WriteLine(Name);
            if (ColumnModel == null && Name != null)
            {
                ColumnModel = RunningConfig.Columns.FirstOrDefault(x => x.Name == Name);
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
            if (args.ShiftKey == false)
            {
                //Get all columns enabled except this one
                var columns = RunningConfig.Columns
                    .Where(x => x.OrderableDirection != OrderableDirection.Disabled)
                    .Where(x => x != ColumnModel)
                    .ToList();

                //reset direction
                foreach (var column in columns)
                {
                    column.OrderableDirection = OrderableDirection.None;
                }
                //clear ordered list
                RunningConfig.ColumnsOrdered.Clear();
            }
            switch (ColumnModel!.OrderableDirection)
            {
                case OrderableDirection.None: ColumnModel.OrderableDirection = OrderableDirection.Ascending; break;
                case OrderableDirection.Ascending: ColumnModel.OrderableDirection = OrderableDirection.Descending; break;
                case OrderableDirection.Descending: ColumnModel.OrderableDirection = OrderableDirection.None; break;
            }
            RunningConfig.ColumnsOrdered.Add(ColumnModel);

            await Engine.UpdateAsync();
        }
    }
}