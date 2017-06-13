using Kadena.WebAPI.Infrastructure.Communication;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using System.Web.Http.Results;

namespace Kadena.WebAPI.Infrastructure
{
    public class ApiControllerBase : ApiController
    {
        protected static JsonSerializerSettings camelCaseSerializer = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatString = "yyyy-MM-dd HH:mm:ss"
        };

        protected JsonResult<ErrorResponse> ErrorJson(string errorMessage)
        {
            return Json(new ErrorResponse(errorMessage), camelCaseSerializer);
        }

        protected JsonResult<BaseResponse<T>> ResponseJson<T>(T payload)
        {
            var response = new BaseResponse<T>()
            {
                Success = true,
                Payload = payload,
                ErrorMessage = null
            };

            return Json(response, camelCaseSerializer);
        }
    }
}