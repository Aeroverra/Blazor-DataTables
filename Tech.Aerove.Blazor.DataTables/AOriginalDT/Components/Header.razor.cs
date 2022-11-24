using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Linq.Dynamic.Core;
using Tech.Aerove.Blazor.DataTables.AOriginalDT.Extensions;
using Tech.Aerove.Blazor.DataTables.AOriginalDT.Models;
using Tech.Aerove.Blazor.DataTables.AOriginalDT.Models.Enums;

namespace Tech.Aerove.Blazor.DataTables.AOriginalDT.Components
{
    /// <summary>
    /// Used by the user to define a table header
    /// </summary>
    public partial class Header : ComponentBase, IDisposable
    {
        /// <summary>
        /// Central communication Between all components
        /// Passed down from the DataTable Component
        /// </summary>
        [CascadingParameter] private TableNetwork Network { get; set; } = null!;

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> InputAttributes { get; set; } = new Dictionary<string, object>();

        [Parameter]
        public string? Name { get; set; } //User Passed Name to specify which header this should be

        [Parameter]
        public RenderFragment? ChildContent { get; set; }//inner content created by user

        private ColumnInfoModel? Model { get; set; }

        private bool Render = false;
        private OrderableDirection Direction = OrderableDirection.None;

        protected override void OnParametersSet()
        {
            //Random rand = new Random();
            //Thread.Sleep(rand.Next(1000, 5000));
            Console.WriteLine(Name);
            if (Model == null && Name != null)
            {
                Model = Network.Columns.Single(x => x.Name == Name);
            }
            if (Model != null && Model.Orderable)
            {
                InputAttributes.AddStyleClass("orderable");
                SetOrderClass(Direction.GetClass());
                Network.TableData.OnOrderReset += OnOrderReset;
            }
        }

        public void OnOrderReset(object? sender, EventArgs args)
        {
            Direction = OrderableDirection.None;
            SetOrderClass(Direction.GetClass());
        }

        private async Task ChangeDirectionAsync(MouseEventArgs args)
        {
            if (Model is null)
            {
                return;
            }

            var currentDirection = Direction;

            if (args.ShiftKey == false)
            {
                Network.TableData.ResetOrder();
            }


            switch (currentDirection)
            {
                case OrderableDirection.None: Direction = OrderableDirection.Ascending; break;
                case OrderableDirection.Ascending: Direction = OrderableDirection.Descending; break;
                case OrderableDirection.Descending: Direction = OrderableDirection.None; break;
            }
            SetOrderClass(Direction.GetClass());

            if (Direction != OrderableDirection.None)
            {
                if (Network.TableData.OrderableCommands.Any(x => x.PropertyName == Model.Name))
                {
                    Network.TableData.OrderableCommands.RemoveAll(x => x.PropertyName == Model.Name);
                }
                Network.TableData.OrderableCommands.Add(new OrderCommand(Model.Name, Direction));
            }

            if (Network.TableData.UpdateAsync is not null)
            {
                await Network.TableData.UpdateAsync();
            }

        }
        private void SetOrderClass(string orderClass)
        {
            InputAttributes.RemoveStyleClass("asc");
            InputAttributes.RemoveStyleClass("desc");
            InputAttributes.AddStyleClass(orderClass);
        }
        public void Dispose()
        {
            if (Render)
            {
                Network.TableData.OnOrderReset -= OnOrderReset;
            }
        }
    }
}