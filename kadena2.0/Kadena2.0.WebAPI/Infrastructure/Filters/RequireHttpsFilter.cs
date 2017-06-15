using System;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Kadena.WebAPI.Infrastructure.Filters
{
    public class RequireHttpsAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext context)
        {
            if (context.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                context.Response = FilterHelper.ErrorResponse("HTTPS required");
            }
            else
            {
                base.OnAuthorization(context);
            }
        }
    }
}