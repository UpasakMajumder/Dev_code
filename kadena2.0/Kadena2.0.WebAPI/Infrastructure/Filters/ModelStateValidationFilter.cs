using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Kadena.WebAPI.Infrastructure.Filters
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var message = context.ModelState.Values?.FirstOrDefault()?.Errors?[0]?.ErrorMessage;

                if (string.IsNullOrEmpty(message))
                    message = "Bad request format";

                context.Response = FilterHelper.ErrorResponse($"Request validation error : {message}");
            }
        }
    }
}