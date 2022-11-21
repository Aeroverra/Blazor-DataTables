using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tech.Aerove.Blazor.DataTables.Extensions
{
    internal static class DictionaryExtensions
    {

        /// <summary>
        /// Determines if a set of html attributes has a specific class
        /// </summary>
        /// <param name="attributes">CaptureUnmatchedValues parameter dictionary</param>
        /// <returns></returns>
        internal static bool HasClass(this Dictionary<string, object>? attributes, string classValue)
        {
            if(attributes == null)
            {
                return false;
            }

            // value of html class attribute
            var classValueString = attributes
                .Where(x => x.Key == "class")
                .Select(x=>x.Value.ToString())
                .FirstOrDefault();

            if(classValueString == null)
            {
                return false;
            }

            var hasClass = classValueString
                .Split(" ")
                .Any(x => x == classValue);

            return hasClass;
        }
    }
}
