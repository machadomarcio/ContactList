using ContactList.Common.Exceptions;
using ContactList.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ContactList.Common.DependencyResolution
{
    public class IoC
    {
        protected IoC()
        {
        }

        private static Dictionary<Type, Type> _instancesTypes;
        private static List<Type> _types;
        private static object _lock = new object();

        public static T Get<T>(params Object[] paramenters)
        {
            return GetInstanceByType<T>();
        }

        private static T GetInstanceByType<T>(params Object[] paramenters)
        {
            if (_instancesTypes == null) _instancesTypes = new Dictionary<Type, Type>();

            if (_types == null) _types = ObjectExtension.GetAllTypes();

            var keyType = typeof(T);

            Type dataType;

            if (!_instancesTypes.TryGetValue(keyType, out dataType))
            {
                lock (_lock)
                {
                    if (!_instancesTypes.TryGetValue(keyType, out dataType))
                    {
                        dataType = _types.FirstOrDefault(p => keyType.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract);

                        _instancesTypes.Add(keyType, dataType);
                    }
                }
            }

            try
            {
                Object outDataObject;

                if (paramenters.Any())
                    outDataObject = Activator.CreateInstance(dataType, paramenters);
                else
                    outDataObject = Activator.CreateInstance(dataType);

                return (T)outDataObject;
            }
            catch (Exception ex)
            {
                if (ex.InnerException is ConnectionStringException)
                    throw;


                return default(T);
            }

        }

        public static Lazy<T> GetLazy<T>(params Object[] paramenters)
        {
            return new Lazy<T>(() => GetInstanceByType<T>(paramenters));
        }
    }
}