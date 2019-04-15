using System;

namespace ContactList.Common.Extensions
{
    public static class Generic
    {
        /// <summary>
        /// Get Value with verification object is null
        /// </summary>
        /// <typeparam name="TObject">Type of the object</typeparam>
        /// <typeparam name="TValue">Type of the value to return</typeparam>
        /// <param name="obj">Obejct to get value</param>
        /// <param name="getAction">Action to get value</param>
        /// <returns>Value acessed to action</returns>
        public static TValue Value<TObject, TValue>(this TObject obj, Func<TObject, TValue> getAction)
        {
            if (obj == null) return default(TValue);

            return getAction(obj);
        }
    }
}
