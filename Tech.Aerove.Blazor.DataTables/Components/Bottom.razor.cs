using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Tech.Aerove.Blazor.DataTables.Api;
using Tech.Aerove.Blazor.DataTables.Configs;
using Tech.Aerove.Blazor.DataTables.Context;

namespace Tech.Aerove.Blazor.DataTables.Components
{
    public partial class Bottom<TItem> : ComponentBase
    {
        [CascadingParameter] internal TableContext<TItem> Context { get; set; } = null!;
        private UIConfig UIConfig => Context.UIConfig;
        private RunningConfig RunningConfig => Context.RunningConfig;
        private Engine<TItem> Engine => Context.Engine;
        private TableController Api => Context.Api;


        private string GetResultdescriptor()
        {
            var to = Api.Start + Api.Length;
            if (Api.RecordsFiltered < to)
            {
                to = Api.RecordsFiltered;
            }
            var start = Api.Start + 1; 
            if(Api.RecordsFiltered == 0)
            {
                start = 0;
            }
            var descriptor = $"Showing {start} to {to} of {Api.RecordsFiltered} entries";
            if (Api.RecordsFiltered != Api.RecordsTotal)
            {
                descriptor += $" (filtered from {Api.RecordsTotal} total entries)";
            }
            return descriptor;
        }

        internal  Task SetPageNextAsync()
        {
            Api.SetPageNext();
            return Api.UpdateAsync();
        }

        internal Task SetPagePreviousAsync()
        {
            Api.SetPagePrevious();
            return Api.UpdateAsync();
        }

        internal Task SetPageFirstAsync()
        {
            Api.SetPageFirst();
            return Api.UpdateAsync();
        }

        internal Task SetPageLastAsync()
        {
            Api.SetPageLast();
            return Api.UpdateAsync();
        }
    }
}