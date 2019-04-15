using System;

namespace ContactList.Domain.Service.Exceptions
{
    public class ContactListException : ApplicationException
    {
        public ContactListException(string message) : base(message)
        {
        }
    }
}