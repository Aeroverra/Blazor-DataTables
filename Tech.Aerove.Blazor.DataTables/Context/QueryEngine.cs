using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech.Aerove.Blazor.DataTables.Context
{
    internal static class QueryEngine
    {
        internal static Task<IQueryable<TItem>> OrderAsync<TItem>(this IQueryable<TItem> query)
        {
            throw new NotImplementedException();
        }

        internal static Task<IQueryable<TItem>> FilterAsync<TItem>(this IQueryable<TItem> query)
        {
            throw new NotImplementedException();
        }

        internal static Task<IQueryable<TItem>> SearchAsync<TItem>(this IQueryable<TItem> query)
        {
            throw new NotImplementedException();
        }

        internal static Task<int> CountAsync<TItem>(this IQueryable<TItem> query)
        {
            if (query is IAsyncQueryProvider)
            {
                return query.CountAsync();
            }
            else
            {
                return Task.FromResult(query.Count());
            }
        }
    }
}
