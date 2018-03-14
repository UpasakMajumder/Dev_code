using System;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using Kadena.Container.Default;

namespace Kadena.WebAPI.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    // Authorization filter runs first so it will log even requests without fields marked as mandatory
    public class RequestLogFilter : AuthorizationFilterAttribute 
    {
        public IKenticoLogger Logger = DIContainer.Resolve<IKenticoLogger>();
        private string source = "WebAPI";

        public RequestLogFilter(string source = "")
        {
            if (!string.IsNullOrEmpty(source))
            {
                this.source = source;
            }
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            Log3dsiRequest(actionContext.Request);
            base.OnAuthorization(actionContext);
        }

        private void Log3dsiRequest(HttpRequestMessage request)
        {
            var log = new StringBuilder();
            log.AppendLine(request?.ToString());
            var body = request?.Content?.ReadAsStringAsync().Result;

            if (!string.IsNullOrEmpty(body))
            {
                log.AppendLine("Body: ");
                log.AppendLine(body);
            };

            Logger.LogInfo(source, "HTTP request", log.ToString());
        }
    }
 }