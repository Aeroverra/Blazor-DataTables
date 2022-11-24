using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech.Aerove.Blazor.DataTables.Context
{

    internal class Engine<TItem> : IEngine
    {
        private readonly TableContext<TItem> Context;
        private readonly QueryEngine QueryEngine = new QueryEngine();
        private UIConfig UIConfig => Context.UIConfig;
        private RunningConfig RunningConfig => Context.RunningConfig;
        internal List<TItem> Items = new List<TItem>();

        public Engine(TableContext<TItem> tableContext)
        {
            Context = tableContext;
        }

        public async Task UpdateAsync()
        {
            var query = await Context.StartQueryAsync();
            //todo: filter, searches and orders
            query = await Context.BeforeFinishQueryAsync(query);


            if (query is IAsyncQueryProvider)
            {
                Items = await query.ToListAsync();
            }
            else
            {
                Items = query.ToList();
            }
            await Context.AfterQueryAsync(true);
        }


    }
}
