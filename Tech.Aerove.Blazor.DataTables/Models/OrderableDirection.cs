using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech.Aerove.Blazor.DataTables.Models
{
    internal enum OrderableDirection
    {
        None,
        Ascending,
        Descending
    }
    internal static class OrderableDirectionExtensions
    {
        public static string GetClass(this OrderableDirection direction)
        {
            switch (direction)
            {
                case OrderableDirection.None: return "";
                case OrderableDirection.Ascending: return "asc";
                case OrderableDirection.Descending: return "desc";
            }
            return "";
        }
    }
}
