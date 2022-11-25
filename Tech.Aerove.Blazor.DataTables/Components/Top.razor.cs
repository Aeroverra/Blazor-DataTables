using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Tech.Aerove.Blazor.DataTables.Configs;
using Tech.Aerove.Blazor.DataTables.Context;

namespace Tech.Aerove.Blazor.DataTables.Components
{
    public partial class Top<TItem> : ComponentBase
    {
        [CascadingParameter] internal TableContext<TItem> Context { get; set; } = null!;
        private UIConfig UIConfig => Context.UIConfig;
        private RunningConfig RunningConfig => Context.RunningConfig;
        private Engine<TItem> Engine => Context.Engine;


        /// <summary>
        /// Called when the user changes the length via the selectlist
        /// </summary>
        private async Task OnLengthChangeAsync(ChangeEventArgs args)
        {
            var value = int.Parse($"{args.Value}");

            //Make sure the user isn't manipulating the values
            if (!UIConfig.Lengths.Contains(value))
            {
                return;
            }
            RunningConfig.Length = value;
            await Engine.UpdateAsync();
        }

        /// <summary>
        /// Called when the user changes the search text in the search input component
        /// </summary>
        private async Task OnSearchChange(ChangeEventArgs args)
        {
            RunningConfig.SearchText = $"{args.Value}";
            await Engine.UpdateAsync();

        }
    }
}