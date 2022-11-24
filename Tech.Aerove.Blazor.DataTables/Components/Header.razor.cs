using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Tech.Aerove.Blazor.DataTables.Context;
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

        }
        private Task ChangeDirectionAsync(MouseEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
}