using Kadena.WebAPI.Infrastructure.Communication;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

using static Kadena.Helpers.SerializerConfig;

namespace Kadena.WebAPI.Infrastructure.Filters
{
    public static class FilterHelper
    {
        public static HttpResponseMessage ErrorResponse(string errorMessage)
        {
            var responseObject = new ErrorResponse(errorMessage);
            var responsejson = JsonConvert.SerializeObject(responseObject, CamelCaseSerializer);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responsejson , Encoding.UTF8, "application/json")
            };
        }
    }
}