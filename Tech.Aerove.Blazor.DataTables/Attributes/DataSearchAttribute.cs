namespace Tech.Aerove.Blazor.DataTables.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataSearchAttribute : Attribute
    {

        /// <summary>
        /// aka == or contains
        /// </summary>
        public bool Exact { get; private set; }

        public DataSearchAttribute(bool exact)
        {
            Exact = exact;
        }

    }
}
