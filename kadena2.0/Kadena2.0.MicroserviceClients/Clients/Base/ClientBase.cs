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

        protected async Task<AwsResponseMessage<TResult>> ReadResponseJson<TResult>(HttpResponseMessage response)
        {
            AwsResponseMessage<TResult> result = null;

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                result = new AwsResponseMessage<TResult>
                {
                    Success = false,
                    Payload = default(TResult),
                    Error = new ErrorMessage
                    {
                        Message = response.ReasonPhrase
                    }
                };
            }
            else
            {
                try
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<AwsResponseMessage<TResult>>(responseContent);
                }
                catch (JsonReaderException e)
                {
                    result = new AwsResponseMessage<TResult>
                    {
                        Success = false,
                        Payload = default(TResult),
                        Error = new ErrorMessage
                        {
                            Message = _responseIncorrectMessage,
                            InnerError = new ErrorMessage
                            {
                                Message = e.Message
                            }
                        }
                    };
                }
            }
            return result;
        }
    }
}
