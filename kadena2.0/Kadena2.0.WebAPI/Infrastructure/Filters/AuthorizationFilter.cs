using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Kadena.WebAPI.Infrastructure.Filters
{
    public class AuthorizationFilter : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext context)
        {
            bool authorized = true; // TODO

            if (authorized)
                return;

            context.Response = FilterHelper.ErrorResponse("Unauthorized access");
        }
    }
}