using System;

namespace ContactList.Domain.Attributes
{
    public class ValueAttribute : Attribute
    {
        public string Value { get; private set; }

        public ValueAttribute(string value)
        {
            Value = value;
        }
    }
}
