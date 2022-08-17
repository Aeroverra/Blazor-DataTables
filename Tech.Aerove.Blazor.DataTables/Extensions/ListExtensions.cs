using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Tech.Aerove.Blazor.DataTables.Models;

namespace Tech.Aerove.Blazor.DataTables.Extensions
{
    internal static class ListExtensions
    {
        public static IQueryable OrderQuery(this List<OrderCommand> orderables, IQueryable query)
        {
            if (orderables.Count() == 0) { return query; }

            OrderableDirection currentDirection = orderables[0].Direction;
            for (var x = 0; x < orderables.Count(); x++)
            {
                var name = orderables[x].PropertyName;
                var direction = orderables[x].Direction.GetClass();
                if (x == 0)
                {
                    query = query.OrderBy($"{name} {direction}");
                    continue;
                }
                query = ((IOrderedQueryable)query).ThenBy($"{name} {direction}");
            }
            return query;
        }
    }
}
