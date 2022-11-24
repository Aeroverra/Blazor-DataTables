using Tech.Aerove.Blazor.DataTables.AOriginalDT.Models.Enums;

namespace Tech.Aerove.Blazor.DataTables.AOriginalDT.Models
{

    /// <summary>
    /// Used to generate a orderedqueryable when querying the data source
    /// </summary>
    internal class OrderCommand
    {
        public string PropertyName { get; set; }
        public OrderableDirection Direction { get; set; }


        public OrderCommand(string propertyName, OrderableDirection direction)
        {
            PropertyName = propertyName;
            Direction = direction;
        }
    }
}
