using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech.Aerove.Blazor.DataTables.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class DataOrderableAttribute : Attribute
    {
        public bool Orderable { get; private set; }

        public DataOrderableAttribute(bool orderable)
        {
            Orderable = orderable;
        }
    }
}
