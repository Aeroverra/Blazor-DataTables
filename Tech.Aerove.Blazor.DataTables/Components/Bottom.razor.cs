using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Tech.Aerove.Blazor.DataTables.Context;

namespace Tech.Aerove.Blazor.DataTables.Components
{
    public partial class Bottom<TItem> : ComponentBase
    {
        [CascadingParameter] internal TableContext<TItem> Context { get; set; } = null!;
        private UIConfig UIConfig => Context.UIConfig;
        private RunningConfig RunningConfig => Context.RunningConfig;
        private Engine<TItem> Engine => Context.Engine;


        private string GetResultdescriptor()
        {
            var to = RunningConfig.Start + RunningConfig.Length;
            if (RunningConfig.RecordsFiltered < to)
            {
                to = RunningConfig.RecordsFiltered;
            }
            var descriptor = $"Showing {RunningConfig.Start + 1} to {to} of {RunningConfig.RecordsFiltered} entries";
            if (RunningConfig.RecordsFiltered != RunningConfig.RecordsTotal)
            {
                descriptor += $"(filtered from {RunningConfig.RecordsTotal} total entries)";
            }
            return descriptor;
        }
        internal async Task SetPageAsync(int page)
        {
            RunningConfig.Page = page;
            RunningConfig.Start = (RunningConfig.Page - 1) * RunningConfig.Length;
            await Engine.UpdateAsync();
        }
        internal Task SetPageNextAsync()
        {
            return SetPageAsync(RunningConfig.Page + 1);
        }

        internal Task SetPagePreviousAsync()
        {
            return SetPageAsync(RunningConfig.Page - 1);
        }

        internal Task SetPageFirstAsync()
        {
            return SetPageAsync(1);
        }

        internal Task SetPageLastAsync()
        {
            return SetPageAsync(RunningConfig.TotalPages);
        }
    }
}