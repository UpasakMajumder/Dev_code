using Kadena.WebAPI.Infrastructure.Communication;
using System.Web.Http;
using System.Web.Http.Results;

using static Kadena.Helpers.SerializerConfig;

namespace Kadena.WebAPI.Infrastructure
{
    public class ApiControllerBase : ApiController
    {
        protected JsonResult<ErrorResponse> ErrorJson(string errorMessage)
        {
            return Json(new ErrorResponse(errorMessage), CamelCaseSerializer);
        }

        protected JsonResult<BaseResponse<object>> SuccessJson()
        {
            return ResponseJson((object)null);
        }

        protected JsonResult<BaseResponse<T>> ResponseJson<T>(T payload)
        {
            var response = new BaseResponse<T>()
            {
                Success = true,
                Payload = payload,
                ErrorMessage = null
            };

            return Json(response, CamelCaseSerializer);
        }

        /// <summary>
        /// If payload is not null, returns BaseResponse 
        /// If payload is null, returns ErrorResponse with given errorMessage
        /// </summary>
        /// <param name="errorMessage"></param>
        protected JsonResult<BaseResponse<T>> ResponseJsonCheckingNull<T>(T payload, string errorMessage) 
        {
            var response = new BaseResponse<T>()
            {
                Success = payload != null,
                Payload = payload,
                ErrorMessage = payload == null ? errorMessage : null
            };

            return Json(response, CamelCaseSerializer);
        }
    }
}