using RestSharp;
using System;
using System.Text;

namespace ContactList.Domain.Service.Exceptions
{
    public class WrapperException : ApplicationException
    {
        private readonly string _errorMessage;

        public WrapperException(string message)
        {
            _errorMessage = message;
        }

        public WrapperException(IRestResponse restResponse)
        {
            StringBuilder stringBuilderErrorMessage = new StringBuilder();
            stringBuilderErrorMessage.AppendLine($"ResponseStatus: {restResponse.ResponseStatus}");
            stringBuilderErrorMessage.AppendLine($"Content: {restResponse.Content}");
            stringBuilderErrorMessage.AppendLine($"Exception: {restResponse.ErrorException}");
            stringBuilderErrorMessage.AppendLine($"ErrorMessage: {restResponse.ErrorMessage}");
            stringBuilderErrorMessage.AppendLine($"StatusCode: {restResponse.StatusCode}");
            stringBuilderErrorMessage.AppendLine($"StatusDescription: {restResponse.StatusDescription}");

            _errorMessage = stringBuilderErrorMessage.ToString();
        }

        public override string Message
        {
            get
            {
                return _errorMessage;
            }
        }
    }
}
