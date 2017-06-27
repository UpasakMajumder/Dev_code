using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Kadena.WebAPI.Infrastructure.Filters
{
    public class QuerystringParameterRequiredAttribute : ActionFilterAttribute
    {
        private string ParameterName { get; set; }

        public QuerystringParameterRequiredAttribute(string parameter)
        {
            this.ParameterName = parameter;
        }

        public override void OnActionExecuting(HttpActionContext context)
        {
            var queryString = context.Request.GetQueryNameValuePairs();

            if (queryString.Any(q => q.Key == ParameterName))
            {
                return;
            }

            context.Response = FilterHelper.ErrorResponse($"Request validation error : Missing query parameter {ParameterName}");
        }
    }
}