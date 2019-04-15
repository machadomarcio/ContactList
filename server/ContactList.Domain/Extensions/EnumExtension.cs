using ContactList.Domain.Attributes;
using System;
using System.ComponentModel;

namespace ContactList.Domain.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescription<T>(this T value) where T : struct
        {
            var type = typeof(T);

            if (!type.IsEnum) return null;

            var fi = type.GetField(value.ToString());

            if (fi == null) return string.Empty;

            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(fi, typeof(DescriptionAttribute));

            return attribute?.Description ?? value.ToString();
        }

        public static string GetValue<T>(this T value) where T : struct
        {
            var type = typeof(T);

            if (!type.IsEnum) return null;

            var field = type.GetField(value.ToString());

            if (field == null) return string.Empty;

            var attribute = (ValueAttribute)Attribute.GetCustomAttribute(field, typeof(ValueAttribute));

            return attribute?.Value;
        }
    }
}