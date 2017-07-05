using CMS.Ecommerce;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Kadena.WebAPI.Infrastructure.Filters
{
    public class AuthorizationFilter : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext context)
        {
            var customer = ECommerceContext.CurrentCustomer;
            if (customer != null)
                return;

            context.Response = FilterHelper.ErrorResponse("Unauthorized access");
        }
    }
}