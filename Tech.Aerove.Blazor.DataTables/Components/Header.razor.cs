using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Tech.Aerove.Blazor.DataTables.Models;

namespace Tech.Aerove.Blazor.DataTables.Components
{
    public partial class Header : IDisposable
    {
        [CascadingParameter]
        private ColumnInfoModel Model { get; set; } = null!; 


        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? InputAttributes { get; set; }

        [Parameter]
        public string? Name { get; set; } //User Passed Name to specify which header this should be

        [Parameter]
        public RenderFragment? ChildContent { get; set; }//inner content created by user

        private bool Render = false;
        private OrderableDirection Direction = OrderableDirection.None;

        protected override void OnInitialized()
        {
            if (Name == Model.Name || Name == null)
            {
                Render = true;
                Model.TableData.OnOrderReset += OnOrderReset;
            }
            base.OnInitialized();
        }

        public void OnOrderReset(object? sender, EventArgs args)
        {
            Direction = OrderableDirection.None;
        }

        private async Task ChangeDirectionAsync(MouseEventArgs args)
        {
            var currentDirection = Direction;

            if (args.ShiftKey == false)
            {
                Model.TableData.ResetOrder();
            }


            switch (currentDirection)
            {
                case OrderableDirection.None: Direction = OrderableDirection.Ascending; break;
                case OrderableDirection.Ascending: Direction = OrderableDirection.Descending; break;
                case OrderableDirection.Descending: Direction = OrderableDirection.None; break;
            }

            if(Direction != OrderableDirection.None)
            {
                if (Model.TableData.OrderableCommands.Any(x => x.PropertyName == Model.Name))
                {
                    Model.TableData.OrderableCommands.RemoveAll(x => x.PropertyName == Model.Name);
                }
                Model.TableData.OrderableCommands.Add(new OrderCommand(Model.Name, Direction));
            }

            await Model.TableData?.UpdateAsync();
        }

        public void Dispose()
        {
            if (Render)
            {
                Model.TableData.OnOrderReset -= OnOrderReset;
            }
        }
    }
}