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
        internal static bool HasStyleClass(this Dictionary<string, object>? attributes, string classValue)
        {
            if (attributes == null)
            {
                return false;
            }

            // value of html class attribute
            var classValueString = attributes
                .Where(x => x.Key == "class")
                .Select(x => x.Value.ToString())
                .FirstOrDefault();

            if (classValueString == null)
            {
                return false;
            }

            var hasClass = classValueString
                .Split(" ")
                .Any(x => x == classValue);

            return hasClass;
        }

        /// <summary>
        /// Adds a class value to your capture all attributes. If it exists already a duplicate won't be added.
        /// </summary>
        /// <param name="attributes">CaptureUnmatchedValues parameter dictionary</param>
        internal static void AddStyleClass(this Dictionary<string, object> attributes, string classValue)
        {
            if (attributes.HasStyleClass(classValue))
            {
                return;
            }

            var attributeClass = attributes
                .Where(x => x.Key == "class")
                .Select(x => x.Value.ToString())
                .SingleOrDefault();

            if (attributeClass == null)
            {
                attributeClass = "";
            }
            attributeClass = $"{attributeClass} {classValue}";
            attributes.Remove("class");
            attributes.Add("class", attributeClass);

        }

        /// <summary>
        /// Removes a class value to your capture all attributes if it exists
        /// </summary>
        /// <param name="attributes">CaptureUnmatchedValues parameter dictionary</param>
        internal static void RemoveStyleClass(this Dictionary<string, object> attributes, string classValue)
        {
            if (attributes.HasStyleClass(classValue))
            {
                return;
            }

            var attributeClassList = attributes
                .Where(x => x.Key == "class")
                .Select(x => $"{x.Value}")
                .Single()
                .Split(" ")
                .Where(x => x.ToLower() != classValue.ToLower())
                .ToList();

            var attributeClass = string.Join(" ", attributeClassList);

            attributes.Remove("class");
            attributes.Add("class", attributeClass);

        }
    }

}
