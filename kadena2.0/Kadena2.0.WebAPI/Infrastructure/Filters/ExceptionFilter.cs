using System;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Web.Http.Filters;
using Kadena.Infrastructure.Exceptions;

namespace Kadena.WebAPI.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        IKenticoLogger logger;
        readonly string loggerSource = "WEBAPI";

        public ExceptionFilter()
        {
            logger = new KenticoProviders.KenticoLogger();
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            context.Response = FilterHelper.ErrorResponse(context.Exception.Message);

            var webApiException = context.Exception as WebApiException;

            if (webApiException?.LogInEventLog ?? true)
            {
                logger?.LogException(loggerSource, context.Exception);
            }
        }
    }
}