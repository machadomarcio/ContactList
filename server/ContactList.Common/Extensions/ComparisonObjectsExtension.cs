using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ContactList.Common.Results;
using KellermanSoftware.CompareNetObjects;

namespace ContactList.Common.Extensions
{
    public static class ComparisonObjectsExtension
    {
        /// <summary>
        /// Compare two lists ignoring "Ids" and complex objects
        /// Return if lists are equal
        /// </summary>
        /// <typeparam name="T">Type Object</typeparam>
        /// <param name="firstList">First list for compare</param>
        /// <param name="secondList">Second list for compare</param>
        /// <returns>Return if lists are equal</returns>
        public static bool IsDifferent<T>(this List<T> firstList, List<T> secondList)
        {
            CompareLogic comparer = new CompareLogic();

            var props = firstList.GetType().GetGenericArguments().First().GetProperties();

            comparer.Config.IgnoreCollectionOrder = true;

            comparer.Config.MembersToIgnore.AddRange(props.Select(p => p.Name).Where(n => n.EndsWith("Id")).ToList());

            comparer.Config.MembersToIgnore.AddRange(props
                .Where(p => !p.PropertyType.Namespace.StartsWith("System") || p.PropertyType.Namespace.StartsWith("System.Collections"))
                .Select(p => p.Name).ToList());

            return !comparer.Compare(firstList, secondList).AreEqual;
        }

        /// <summary>
        /// Compare two objects with their children
        /// Return if objects are equal
        /// </summary>
        /// <typeparam name="T">Type Object</typeparam>
        /// <param name="firstObject">First object for compare</param>
        /// <param name="secondObject">Second object for compare</param>
        /// <returns>Return if objects are equal</returns>
        public static bool AreEqual<T>(this T firstObject, T secondObject)
        {
            CompareLogic comparer = new CompareLogic();

            comparer.Config.IgnoreCollectionOrder = true;

            return comparer.Compare(firstObject, secondObject).AreEqual;
        }

        /// <summary>
        ///  Compare objects ignoring "Ids" and complex objects. Sets the current value if the objects are different 
        ///  Returns if there were changes in the object
        /// </summary>
        /// <typeparam name="T">Type object</typeparam>
        /// <param name="oldObj">Old object for compare</param>
        /// <param name="newObj">New object for compare</param>
        /// <returns>Returns if there were changes in the object</returns>
        public static bool UpdatePropertiesOfObject<T>(this T oldObj, T newObj)
        {
            PropertyInfo[] olderProperties = oldObj.GetType()
                .GetProperties()
                .Where(x => !x.Name.EndsWith("Id") && x.PropertyType.Namespace.StartsWith("System") && !x.PropertyType.Namespace.StartsWith("System.Collections") || x.Name.Equals("ActivityBranch"))
                .ToArray();

            PropertyInfo[] newProperties = newObj.GetType().GetProperties();

            var countProperties = olderProperties.Length;

            var isModified = false;

            for (int i = 0; i < countProperties; i++)
            {
                var olderProperty = olderProperties[i];

                var newProperty = newProperties.FirstOrDefault(x => x.Name == olderProperty.Name);

                if (newProperty == null) continue;

                var olderValue = olderProperty.GetValue(oldObj);

                var newValue = newProperty.GetValue(newObj);

                if (olderValue == null || !olderValue.Equals(newValue))
                {
                    olderProperty.SetValue(oldObj, newValue);
                    isModified = true;

                    if (olderValue == null && newValue == null)
                        isModified = false;
                }

            }

            return isModified;
        }

        /// <summary>
        /// Compare two objects with their children
        /// Returns the comparison result
        /// </summary>
        /// <typeparam name="T">Type Object</typeparam>
        /// <param name="firstObject">First object for compare</param>
        /// <param name="secondObject">Second object for compare</param>
        /// <param name="propertiesToIgnore">Array of property names to ignore</param>
        /// <returns>Return the comparison result</returns>
        public static ObjectComparisonResult Compare<T>(this T firstObject, T secondObject, string[] propertiesToIgnore = null)
        {
            var comparer = new CompareLogic();
            var props = firstObject.GetType().GetProperties();
            comparer.Config.IgnoreCollectionOrder = true;
            comparer.Config.MaxDifferences = props.Length;
            comparer.Config.IgnoreObjectTypes = true;

            if (propertiesToIgnore != null && propertiesToIgnore.Length > 0)
            {

                foreach (var property in propertiesToIgnore)
                {
                    comparer.Config.MembersToIgnore.AddRange(props.Select(p => p.Name).Where(n => n == property).ToList());
                }
            }


            var result = comparer.Compare(firstObject, secondObject);

            return new ObjectComparisonResult
            {
                AreEqual = result.AreEqual,
                Differences = result.Differences.Select(x => new ObjectComparisonDiffernce
                {
                    PropertyName = x.PropertyName.TrimStart(new char[] { '.' }),
                    Object1Value = x.Object1Value,
                    Object2Value = x.Object2Value
                }).ToList()
            };
        }
    }
}
