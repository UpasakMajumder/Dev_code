using Kadena.Dto.General;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients.Base
{
    public class ClientBase
    {
        private const string _responseIncorrectMessage = "Response from microservice is not in correct format.";

        protected static JsonSerializerSettings camelCaseSerializer = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatString = "yyyy-MM-dd HH:mm:ss"
        };

        protected StringContent CreateRequestContent(object request)
        {
            var requestBody = JsonConvert.SerializeObject(request, camelCaseSerializer);
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
            return content;
        }

        protected async Task<BaseResponseDto<TResult>> ReadResponseJson<TResult>(HttpResponseMessage response)
        {
            BaseResponseDto<TResult> result = null;
            BaseErrorDto innerError = null;
            string responseContent = string.Empty;

            // In these cases, there may be JSON with standard structure and proper error message from microservice
            if (response.StatusCode == HttpStatusCode.OK || 
                response.StatusCode == HttpStatusCode.BadRequest || 
                response.StatusCode == HttpStatusCode.Unauthorized || 
                response.StatusCode == HttpStatusCode.BadGateway ||
                response.StatusCode == HttpStatusCode.NotImplemented ||
                response.StatusCode == HttpStatusCode.InternalServerError)
            {
                responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(responseContent))
                {
                    try
                    {
                        result = JsonConvert.DeserializeObject<BaseResponseDto<TResult>>(responseContent);
                    }
                    catch (Exception e)
                    {
                        innerError = new BaseErrorDto { Message = $"{e.Message} response content: '{responseContent}'" };
                    }
                }
            }
            else // some severe network error
            {
                result = new BaseResponseDto<TResult>
                {
                    Success = false,
                    Payload = default(TResult),
                    Error = new BaseErrorDto
                    {
                        Message = $"HttpClient received status {response.StatusCode}, reason {response.ReasonPhrase}"
                    }
                };
            }

            return result ?? new BaseResponseDto<TResult>
                                {
                                    Success = false,
                                    Payload = default(TResult),
                                    Error = new BaseErrorDto
                                    {
                                        Message = $"{_responseIncorrectMessage} response content: '{responseContent}'",
                                        InnerError = innerError
                                    }
                                };
        }
    }
}
