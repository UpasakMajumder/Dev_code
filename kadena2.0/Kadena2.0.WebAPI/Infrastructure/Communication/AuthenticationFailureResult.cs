using Kadena.Models.Common;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

using static Kadena.Helpers.SerializerConfig;

namespace Kadena.WebAPI.Infrastructure.Communication
{
    public class AuthenticationFailureResult : IHttpActionResult
    {
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            ReasonPhrase = reasonPhrase;
            Request = request;
        }

        public string ReasonPhrase { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            var errorDto = new ErrorResponse(ReasonPhrase);
            var requestBody = JsonConvert.SerializeObject(errorDto, CamelCaseSerializer);
            var content = new StringContent(requestBody, Encoding.UTF8, ContentTypes.Json);

            return new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                RequestMessage = Request,
                Content = content
            };
        }
    }
}