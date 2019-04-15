using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ContactList.Common.Extensions
{
    public static class ObjectExtension
    {
        public static List<Type> GetAllTypes()
        {
            var listTypes = new List<Type>();

            var listAssemblies = GetAllAssemblies();

            listAssemblies.ForEach(x => listTypes.AddRange(x.GetTypes()));
            
            return listTypes;
        }

        private static List<Assembly> GetAllAssemblies()
        {
            var listAssemblies = new List<Assembly>();
            
            var path = AppDomain.CurrentDomain.RelativeSearchPath;

            if (path == null)
                path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            
            var files = Directory.GetFiles(path, "ContactList*.*");

            foreach (var file in files)
            {
                if (!file.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase) &&
                    !file.EndsWith(".exe", StringComparison.CurrentCultureIgnoreCase)) continue;

                var assembly = Assembly.LoadFrom(file);

                listAssemblies.Add(assembly);
            }

            return listAssemblies;
        }

        public static T CloneObject<T>(this Object currentObj, Type newType)
        {
            PropertyInfo[] fields = newType.GetProperties();

            FieldInfo[] databaseFields = currentObj.GetType().GetFields();

            Object newInstance = newType.InvokeMember("", BindingFlags.CreateInstance, null, currentObj, null);

            foreach (PropertyInfo fi in fields)
            {
                fi.SetValue(newInstance, databaseFields.FirstOrDefault(x => x.Name == fi.Name)?.GetValue(currentObj) ?? fi.GetValue(newInstance));
            }

            return (T)newInstance;
        }
    }
}
