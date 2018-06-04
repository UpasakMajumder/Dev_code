namespace Kadena.Infrastructure.Exceptions
{
    public class NotLoggedException : WebApiException
    {
        public NotLoggedException(string message) : base(message)
        {
            LogInEventLog = false;
        }
    }
}
