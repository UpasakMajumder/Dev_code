using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Contracts.Base;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using static Kadena.Helpers.SerializerConfig;

namespace Kadena2.MicroserviceClients.Clients.Base
{
    public abstract class ClientBase
    {
        protected IMicroProperties _properties;
        private readonly ISuppliantDomainClient _suppliantDomain;
        private readonly string _baseServiceUrlSettingKey = "KDA_MicroservicesBaseAddress";
        protected string _serviceUrlSettingKey;
        protected string _serviceVersionSettingKey;

        public ClientBase()
        {

        }

        protected ClientBase(ISuppliantDomainClient suppliantDomain) : this()
        {
            _suppliantDomain = suppliantDomain;
        }

        private const string _responseIncorrectMessage = "Response from microservice is not in correct format.";

        public string BaseUrl => _properties.GetServiceUrl(_serviceUrlSettingKey);

        public string NewBaseUrl
        {
            get
            {
                var url = _properties.GetServiceUrl(_baseServiceUrlSettingKey);
                var version = _properties.GetServiceUrl(_serviceVersionSettingKey);
                return $"{url}/v{version}";
            }
        }

        protected async Task<BaseResponseDto<TResult>> Get<TResult>(string url)
        {
            return await Send<TResult>(HttpMethod.Get, url).ConfigureAwait(false); ;
        }

        protected async Task<BaseResponseDto<TResult>> Post<TResult>(string url, object body)
        {
            return await Send<TResult>(HttpMethod.Post, url, body).ConfigureAwait(false); ;
        }

        protected async Task<BaseResponseDto<TResult>> Delete<TResult>(string url, object body = null)
        {
            return await Send<TResult>(HttpMethod.Delete, url, body).ConfigureAwait(false); ;
        }

        protected async Task<BaseResponseDto<TResult>> Patch<TResult>(string url, object body)
        {
            return await Send<TResult>(new HttpMethod("PATCH"), url, body).ConfigureAwait(false); ;
        }

        protected async Task<BaseResponseDto<TResult>> Put<TResult>(string url, object body)
        {
            return await Send<TResult>(HttpMethod.Put, url, body).ConfigureAwait(false); ;
        }

        protected static string SerializeRequestContent(object body)
        {
            return JsonConvert.SerializeObject(body, CamelCaseSerializer);
        }

        protected virtual async Task<BaseResponseDto<TResult>> SendRequest<TResult>(HttpRequestMessage request)
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.SendAsync(request).ConfigureAwait(false))
                {
                    return await ReadResponseJson<TResult>(response).ConfigureAwait(false);
                }
            }
        }

        protected virtual HttpRequestMessage CreateRequest(HttpMethod method, string url, object body = null)
        {
            var request = new HttpRequestMessage(method, url);
            AddSuppliantDomain(request);

            if (body != null)
            {
                request.Content = new StringContent(SerializeRequestContent(body), Encoding.UTF8, "application/json");
            }
            return request;
        }

        protected virtual async Task<BaseResponseDto<TResult>> ReadResponseJson<TResult>(HttpResponseMessage response)
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

        private async Task<BaseResponseDto<TResult>> Send<TResult>(HttpMethod method, string url, object body = null)
        {
            HttpRequestMessage request = null;
            try
            {
                request = CreateRequest(method, url, body);
                return await SendRequest<TResult>(request).ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                return new BaseResponseDto<TResult>
                {
                    Success = false,
                    Payload = default(TResult),
                    Error = new BaseErrorDto
                    {
                        Message = "Failed to request microservice.",
                        InnerError = new BaseErrorDto
                        {
                            Message = exc.GetBaseException().Message
                        }
                    }
                };
            }
            finally
            {
                request?.Dispose();
            }
        }

        private void AddSuppliantDomain(HttpRequestMessage request)
        {
            var suppliantDomain = _suppliantDomain?.GetSuppliantDomain();
            if (!string.IsNullOrEmpty(suppliantDomain))
            {
                request.Headers.Add("suppliantDomain", suppliantDomain);
            }
        }
    }
}
