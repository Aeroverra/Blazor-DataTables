namespace Tech.Aerove.Blazor.DataTables.AOriginalDT.Models.Enums
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
            throw new NotImplementedException("Unknown direction provided. ");
        }
    }
}
