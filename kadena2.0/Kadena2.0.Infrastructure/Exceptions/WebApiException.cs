using System;

namespace Kadena.Infrastructure.Exceptions
{
    public class WebApiException : Exception
    {
        public bool LogInEventLog { get; protected set; } = true;

        public WebApiException() : base()
        {

        }

        public WebApiException(string message) : base(message)
        {

        }

        public WebApiException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
