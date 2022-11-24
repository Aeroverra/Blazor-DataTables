using Tech.Aerove.Blazor.DataTables.AOriginalDT.Models.Enums;

namespace Tech.Aerove.Blazor.DataTables.AOriginalDT.Attributes
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
