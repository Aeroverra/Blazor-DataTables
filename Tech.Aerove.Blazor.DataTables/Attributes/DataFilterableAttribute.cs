using Tech.Aerove.Blazor.DataTables.Models.Enums;

namespace Tech.Aerove.Blazor.DataTables.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataSearchAttribute : Attribute
    {
        public SearchMode Mode { get; private set; }

        public DataSearchAttribute(SearchMode mode)
        {
            Mode = mode;
        }

    }
}
