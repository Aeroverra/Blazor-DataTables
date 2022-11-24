using System.Reflection;

namespace Tech.Aerove.Blazor.DataTables.AOriginalDT.Attributes
{
    internal class DataLengthAttribute : Attribute
    {
        public bool Enabled = true;
        public List<int> Lengths { get; private set; }

        public DataLengthAttribute(bool enabled, string lengths = "10,25,50,100")
        {
            Enabled = enabled;
            Lengths = lengths.Split(",")
                .Select(x => int.Parse(x.Trim()))
                .ToList();
        }

        public static DataLengthAttribute? Get<TItem>()
        {
            var type = typeof(TItem);
            var lengthAttribute = type.GetCustomAttribute<DataLengthAttribute>();
            return lengthAttribute;
        }

        public static bool IsEnabled<TItem>()
        {
            var enabled = true;
            var lengthAttribute = Get<TItem>();
            if (lengthAttribute != null)
            {
                enabled = lengthAttribute.Enabled;
            }
            return enabled;
        }

        public static List<int> GetLengths<TItem>()
        {
            var lengthAttribute = Get<TItem>();
            return lengthAttribute == null ? new List<int> { 10, 25, 50, 100 } : lengthAttribute.Lengths;
        }
    }
}
