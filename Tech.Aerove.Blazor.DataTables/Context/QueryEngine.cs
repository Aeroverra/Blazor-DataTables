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

    /// <summary>
    /// Handles all the dynamic Linq queries 
    /// </summary>
    internal static class QueryEngine
    {
        /// <summary>
        /// Async Counts the amount of entities if available and if not does a synchronous count
        /// </summary>
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

        /// <summary>
        /// Adds a search expression to the given Queryable and returns the result
        /// </summary>
        internal static IQueryable<TItem> Search<TItem>(this IQueryable<TItem> query, List<ColumnModel> Columns, string searchText)
        {
            //WARNING: This uses params to prevent SQL Injection!
            if (!string.IsNullOrEmpty(searchText) && Columns.Any(x => x.SearchMode != SearchMode.Disabled))
            {
                List<string> searchStrings = new List<string>();
                List<object> searchParams = new List<object>() { searchText };
                foreach (var column in Columns.Where(x => x.SearchMode != SearchMode.Disabled))
                {
                    //Type needs to be added to use this
                    //if (column.Type.IsEnum)
                    //{
                    //    var enumNames = column.Type.GetEnumNames();
                    //    var enumResults = new List<int>();
                    //    if (column.SearchMode == SearchMode.Exact)
                    //    {
                    //        enumResults = enumNames
                    //           .Where(x => x.ToLower() == Network.TableData.SearchInput.ToLower())
                    //           .Select(x => (int)Enum.Parse(column.Type, x))
                    //           .ToList();
                    //    }
                    //    else
                    //    {
                    //        enumResults = enumNames
                    //            .Where(x => x.ToLower().Contains(Network.TableData.SearchInput.ToLower()))
                    //            .Select(x => (int)Enum.Parse(column.Type, x))
                    //            .ToList();
                    //    }

                    //    foreach (var enumResult in enumResults)
                    //    {
                    //        searchStrings.Add($"{column.Name} == {enumResult}");
                    //    }
                    //}
                    //else
                    //{
                        var param = "@0";
                        if (column.SearchMode == SearchMode.Exact)
                        {
                            searchStrings.Add($"{column.Name} == {param}");
                        }
                        else
                        {
                            searchStrings.Add($"{column.Name}.Contains({param})");
                        }
                    //}
                }
                query = query.Where(string.Join(" || ", searchStrings), searchParams.ToArray());
            }
            return query;
        }

        /// <summary>
        /// Adds filter expressions to the given Queryable and returns the result
        /// </summary>
        internal static IQueryable<TItem> Filter<TItem>(this IQueryable<TItem> query, List<ColumnModel> columns)
        {
            //WARNING: This uses params to prevent SQL Injection!
            foreach (var column in columns.Where(x=>x.Filterable).Where(x => x.Filters.Any()))
            {
                List<string> searchStrings = new List<string>();
                for (var x = 0; x < column.Filters.Count(); x++)
                {
                    searchStrings.Add($"{column.Name} == @{x}");
                }
                query = query.Where(string.Join(" || ", searchStrings), column.Filters.ToArray());

            }
            return query;
        }

        /// <summary>
        /// adds an order expression to the given Queryable and returns the result
        /// </summary>
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

        /// <summary>
        /// Gets Async List if available and if not gets a synchronous List
        /// </summary>
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
