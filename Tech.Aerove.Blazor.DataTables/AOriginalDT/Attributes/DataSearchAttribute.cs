namespace Tech.Aerove.Blazor.DataTables.AOriginalDT.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class DataFilterableAttribute : Attribute
    {
        public bool Filterable { get; private set; }

        public DataFilterableAttribute(bool filterable)
        {
            Filterable = filterable;
        }

    }
}
