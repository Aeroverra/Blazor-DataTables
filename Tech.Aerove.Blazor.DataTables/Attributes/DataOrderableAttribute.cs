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
