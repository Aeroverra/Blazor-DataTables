using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech.Aerove.Blazor.DataTables.Enums
{
    public enum OrderableDirection
    {
        Disabled,
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
                case OrderableDirection.Disabled: return "disabled";
                case OrderableDirection.None: return "";
                case OrderableDirection.Ascending: return "asc";
                case OrderableDirection.Descending: return "desc";
            }
            throw new NotImplementedException("Unknown direction provided. ");
        }
    }
}
