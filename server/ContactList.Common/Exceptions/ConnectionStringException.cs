using System;

namespace ContactList.Common.Exceptions
{
    public class ConnectionStringException : ApplicationException
    {
        public ConnectionStringException() : base()
        {

        }

        public ConnectionStringException(string message) : base(message)
        {

        }     
    }
}