using Amazon.Runtime.CredentialManagement;
using Amazon.SecurityToken;
using Kadena.Dto.General;
using Kadena.KOrder.PaymentService.Infrastucture.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients.Base
{
    public abstract class ClientBase
    {
        private const string _responseIncorrectMessage = "Response from microservice is not in correct format.";

        protected static JsonSerializerSettings camelCaseSerializer = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateFormatString = "yyyy-MM-dd HH:mm:ss"
        };

        public bool SignRequest { get; set; } = false;

        public string AwsGatewayApiRole { get; set; } = "arn:aws:...........";

        public string SuppliantDomain { get; set; }

        protected readonly IAwsV4Signer signer;

        public ClientBase()
        {
            //var filePath = SharedCredentialsFile.DefaultFilePath;
            var filePath = @"c:\users\Cenveo\.aws\credentials";

            var file = new SharedCredentialsFile(filePath);
            CredentialProfile prof = null;
            if (!file.TryGetProfile("default", out prof))
            {
                throw new ArgumentException($"Failed to load AWS credentials file");
            }

            CredentialProfileStoreChain source = new CredentialProfileStoreChain();
            var credentials = prof.GetAWSCredentials(source).GetCredentials();
            IAmazonSecurityTokenService service = new AmazonSecurityTokenServiceClient(credentials.AccessKey, credentials.SecretKey ); // TODO or take from Kentico configuration
            this.signer = new DefaultAwsV4Signer(service);
        }

        /*public ClientBase(IAwsV4Signer signer)
        {
            if(signer == null)
            {
                throw new ArgumentNullException(nameof(signer));
            }

            this.signer = signer;
        }*/

        public async Task<BaseResponseDto<TResult>> Get<TResult>(string url)
        {
            return await Send<TResult>(HttpMethod.Get, url);
        }

        public async Task<BaseResponseDto<TResult>> Post<TResult>(string url, object body)
        {
            return await Send<TResult>(HttpMethod.Post, url, body);
        }

        public async Task<BaseResponseDto<TResult>> Delete<TResult>(string url, object body = null)
        {
            return await Send<TResult>(HttpMethod.Delete, url, body);
        }

        public async Task<BaseResponseDto<TResult>> Patch<TResult>(string url, object body)
        {
            return await Send<TResult>(new HttpMethod("PATCH"), url, body);
        }

        public async Task<BaseResponseDto<TResult>> Send<TResult>(HttpMethod method,  string url, object body = null)
        {
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(method, url))
                {
                    if (!string.IsNullOrEmpty(SuppliantDomain))
                    {
                        AddHeader(client, "suppliantDomain", SuppliantDomain);
                    }

                    if (body != null)
                    {
                        request.Content = CreateRequestContent(request, body);
                    }

                    if (SignRequest)
                    {
                        await SignRequestMessage(request);
                    }

                    // TODO consider try-catch ?
                    
                    var response = await client.SendAsync(request);
                    return await ReadResponseJson<TResult>(response);
                }
            }
        }

        private void AddHeader(HttpClient httpClient, string headerName, string headerValue)
        {
            httpClient.DefaultRequestHeaders.Add(headerName, headerValue);
        }

        private async Task SignRequestMessage(HttpRequestMessage request)
        {
            if(string.IsNullOrEmpty(AwsGatewayApiRole))
            {
                throw new ArgumentNullException(nameof(AwsGatewayApiRole), "To use signed request to AWS microservice, you need to provide ApiGatewayRole");
            }

            await signer.SignRequest(request, AwsGatewayApiRole);
        }

        public StringContent CreateRequestContent(HttpRequestMessage request,  object body)
        {
            var requestBody = JsonConvert.SerializeObject(body, camelCaseSerializer);
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
