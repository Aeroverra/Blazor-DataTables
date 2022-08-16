namespace Tech.Aerove.Blazor.DataTables.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DataTableAttribute : Attribute
    {
        public bool DefaultRender { get; private set; } = true;

        public DataTableAttribute(bool defaultRender = true)
        {
            DefaultRender = defaultRender;
        }
    }
}
