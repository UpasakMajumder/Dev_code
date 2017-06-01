using Kadena.WebAPI.Infrastructure.Communication;
using System.Web.Http;
using System.Web.Http.Results;

namespace Kadena.WebAPI.Infrastructure
{
    public class ApiControllerBase : ApiController
    {
        protected JsonResult<ErrorResponse> ErrorJson(string errorMessage)
        {
            return Json(new ErrorResponse(errorMessage), GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings);
        }

        protected JsonResult<BaseResponse<T>> ResponseJson<T>(T payload)
        {
            var response = new BaseResponse<T>()
            {
                Success = true,
                Payload = payload,
                ErrorMessage = null
            };

            return Json(response, GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings);
        }
    }
}