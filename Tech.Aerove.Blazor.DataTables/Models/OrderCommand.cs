using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech.Aerove.Blazor.DataTables.Models
{
    internal class OrderCommand
    {
        public string PropertyName { get; set; }
        public OrderableDirection Direction { get; set; }
  

        public OrderCommand(string propertyName, OrderableDirection direction)
        {
            PropertyName= propertyName;
            Direction = direction;
        }
    }
}
