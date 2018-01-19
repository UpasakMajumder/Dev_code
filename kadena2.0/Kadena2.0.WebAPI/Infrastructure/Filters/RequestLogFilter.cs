using System;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Web.Http.Filters;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

namespace Kadena.WebAPI.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class RequestLogFilter : ActionFilterAttribute
    {
        IKenticoLogger logger;
        readonly string loggerSource = "WEBAPI RequestLog";

        public RequestLogFilter()
        {
            logger = new KenticoProviders.KenticoLogger();
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            Log3dsiRequest(actionExecutedContext.Request);
            base.OnActionExecuted(actionExecutedContext);
        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }

        private void Log3dsiRequest(HttpRequestMessage request)
        {
            var isRequestTo3dsiEndpoint = request?.RequestUri?.AbsoluteUri?.ToLower().Contains("/api/3dsi/approveSubmission") ?? false;

            if (isRequestTo3dsiEndpoint)
            {
                logger.LogInfo(loggerSource, "I", request.ToString());
            }
        }
    }
 }