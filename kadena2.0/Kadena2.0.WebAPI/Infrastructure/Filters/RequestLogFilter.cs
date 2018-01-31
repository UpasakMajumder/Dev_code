﻿using System;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Web.Http.Filters;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using Kadena2.Container.Default;
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;

namespace Kadena.WebAPI.Infrastructure.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    // Authorization filter runs first so it will log even requests without fields marked as mandatory
    public class RequestLogFilter : AuthorizationFilterAttribute 
    {
        private readonly IKenticoLogger logger;
        private string source = "WebAPI";

        public RequestLogFilter(string source = "")
        {
            logger = DIContainer.Resolve<IKenticoLogger>();

            if (!string.IsNullOrEmpty(source))
            {
                this.source = source;
            }
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            Log3dsiRequest(actionContext.Request).Wait();
            base.OnAuthorization(actionContext);
        }

        private async Task Log3dsiRequest(HttpRequestMessage request)
        {
            var log = new StringBuilder();
            log.AppendLine(request?.ToString());
            var body = await request?.Content?.ReadAsStringAsync();

            if (!string.IsNullOrEmpty(body))
            {
                log.AppendLine("Body: ");
                log.AppendLine(body);
            };

            logger.LogInfo(source, "HTTP request", log.ToString());
        }
    }
 }