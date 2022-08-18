namespace Tech.Aerove.Blazor.DataTables.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class DataRenderAttribute : Attribute
    {
        public bool Render { get; private set; } 

        public DataRenderAttribute(bool render)
        {
            Render = render;
        }
    }
}
