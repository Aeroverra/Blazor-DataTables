using Tech.Aerove.Blazor.DataTables.Models.Enums;

namespace Tech.Aerove.Blazor.DataTables.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataFilterableAttribute : Attribute
    {
        public bool Filterable { get; private set; }

        public DataFilterableAttribute(bool filterable)
        {
            Filterable = filterable;
        }

    }
}
