using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Tech.Aerove.Blazor.DataTables.Models;

namespace Tech.Aerove.Blazor.DataTables.Components
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

        protected override void OnParametersSet()
        {
            if (Model.Orderable)
            {
                var attributeClass = InputAttributes?.SingleOrDefault(x => x.Key == "class");
                if (attributeClass == null) { return; }
                if ($"{attributeClass.Value.Value}".Contains("orderable")) { return; }
                InputAttributes?.Remove(attributeClass.Value.Key);
                InputAttributes?.Add(attributeClass.Value.Key, $"{attributeClass.Value.Value} orderable");
                SetOrderClass(Direction.GetClass());
            }
        }

        protected override void OnInitialized()
        {
            if (Name == Model.Name || Name == null)
            {
                Render = true;
                Network.TableData.OnOrderReset += OnOrderReset;
            }
            base.OnInitialized();
        }

        public void OnOrderReset(object? sender, EventArgs args)
        {
            Direction = OrderableDirection.None;
            SetOrderClass(Direction.GetClass());
        }

        private async Task ChangeDirectionAsync(MouseEventArgs args)
        {
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

            await Network.TableData?.UpdateAsync();
        }
        private void SetOrderClass(string orderClass)
        {
            var attributeClass = InputAttributes?.SingleOrDefault(x => x.Key == "class");
            if (attributeClass == null) { return; }
            InputAttributes?.Remove(attributeClass.Value.Key);
            var value = $"{attributeClass.Value.Value}";
            value = value.Replace("asc", "").Replace("desc", "");
            InputAttributes?.Add(attributeClass.Value.Key, $"{value} {orderClass}");
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