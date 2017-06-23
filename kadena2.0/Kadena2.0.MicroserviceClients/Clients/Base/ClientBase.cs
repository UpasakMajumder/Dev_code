using Kadena.Dto.General;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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

        protected async Task<BaseResponse<TResult>> ReadResponseJson<TResult>(HttpResponseMessage response)
        {
            BaseResponse<TResult> result = null;

            try
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<BaseResponse<TResult>>(responseContent);
            }
            catch (JsonReaderException e)
            {
                result = new BaseResponse<TResult>
                {
                    Success = false,
                    Payload = default(TResult),
                    Error = new BaseError
                    {
                        Message = _responseIncorrectMessage,
                        InnerError = new BaseError
                        {
                            Message = e.Message
                        }
                    }
                };
            }
            return result;
        }
    }
}
