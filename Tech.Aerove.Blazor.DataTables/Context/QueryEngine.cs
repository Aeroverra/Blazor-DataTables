using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tech.Aerove.Blazor.DataTables.Enums;
using Tech.Aerove.Blazor.DataTables.Models;

namespace Tech.Aerove.Blazor.DataTables.Context
{
    internal static class QueryEngine
    {

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

        internal static IQueryable<TItem> Search<TItem>(this IQueryable<TItem> query, List<ColumnModel> Columns, string searchText)
        {
            //WARNING: This uses params to prevent SQL Injection!
            if (!string.IsNullOrEmpty(searchText) && Columns.Any(x => x.SearchMode != SearchMode.Disabled))
            {
                List<string> searchStrings = new List<string>();
                List<object> searchParams = new List<object>() { searchText };
                foreach (var column in Columns.Where(x => x.SearchMode != SearchMode.Disabled))
                {
                    var param = "@0";
                    if (column.SearchMode == SearchMode.Exact)
                    {
                        searchStrings.Add($"{column.Name} == {param}");
                    }
                    else
                    {
                        searchStrings.Add($"{column.Name}.Contains({param})");
                    }
                }
                query = query.Where(string.Join(" || ", searchStrings), searchParams.ToArray());
            }
            return query;
        }

        internal static Task<IQueryable<TItem>> FilterAsync<TItem>(this IQueryable<TItem> query)
        {
            throw new NotImplementedException();
        }
        internal static IQueryable<TItem> Order<TItem>(this IQueryable<TItem> query, List<ColumnModel> columns)
        {
            if (columns.Count() == 0) { return query; }


            for (var x = 0; x < columns.Count(); x++)
            {
                var name = columns[x].Name;
                var direction = columns[x].OrderableDirection.GetClass();
                if (x == 0)
                {
                    query = query.OrderBy($"{name} {direction}");
                    continue;
                }
                query = ((IOrderedQueryable<TItem>)query).ThenBy($"{name} {direction}");
            }
            return query;
        }

        internal static Task<List<TItem>> FinishQueryAsync<TItem>(this IQueryable<TItem> query)
        {
            if (query is IAsyncQueryProvider)
            {
                return query.ToListAsync();
            }
            else
            {
                return Task.FromResult(query.ToList());
            }
        }



    }
}
