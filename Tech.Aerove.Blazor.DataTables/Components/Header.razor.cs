using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Tech.Aerove.Blazor.DataTables.Models;

namespace Tech.Aerove.Blazor.DataTables.Components
{
    public partial class Header : IDisposable
    {
        [CascadingParameter]
        private string ColumnName { get; set; } = null!; //name of the column this is rendering

        [CascadingParameter]
        private TableData TableData { get; set; } = null!;

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? InputAttributes { get; set; }

        [Parameter]
        public string? Name { get; set; } //Pass the name 

        [Parameter]
        public RenderFragment? ChildContent { get; set; }//inner content

        private bool Render = false;
        private OrderableDirection Direction = OrderableDirection.None;

        protected override void OnInitialized()
        {
            if (Name == ColumnName || Name == null)
            {
                Render = true;
                TableData.OnOrderReset += OnOrderReset;
            }
            base.OnInitialized();
        }

        public void OnOrderReset(object? sender, EventArgs args)
        {
            Direction = OrderableDirection.None;
        }

        private async Task ChangeDirectionAsync()
        {
            var currentDirection = Direction;
            TableData.ResetOrder();
           
            switch (currentDirection)
            {
                case OrderableDirection.None: Direction = OrderableDirection.Ascending; break;
                case OrderableDirection.Ascending: Direction = OrderableDirection.Descending; break;
                case OrderableDirection.Descending: Direction = OrderableDirection.None; break;
            }
            if(TableData.Orderables.Any(x=>x.PropertyName == ColumnName))
            {
                TableData.Orderables.RemoveAll(x => x.PropertyName == ColumnName);
            }
            TableData.Orderables.Add(new OrderCommand(ColumnName,Direction));
            await TableData?.UpdateAsync();
        }

        public void Dispose()
        {
            if (Render)
            {
                TableData.OnOrderReset -= OnOrderReset;
            }
        }
    }
}