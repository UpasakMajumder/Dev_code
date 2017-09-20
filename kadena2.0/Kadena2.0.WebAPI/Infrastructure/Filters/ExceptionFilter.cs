using System.Web.Http.Filters;

namespace Kadena.WebAPI.Infrastructure.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            context.Response = FilterHelper.ErrorResponse(context.Exception.Message);
        }
    }
}