namespace Tech.Aerove.Blazor.DataTables.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataColumnAttribute : Attribute
    {
        public string? Name { get; private set; }
        public bool? Render { get; private set; }

        public DataColumnAttribute(bool render)
        {
            Render = render;
        }
        public DataColumnAttribute(string name)
        {
            Name = name;
        }
        public DataColumnAttribute(string name, bool render)
        {
            Name = name;
            Render = render;
        }
    }
}
