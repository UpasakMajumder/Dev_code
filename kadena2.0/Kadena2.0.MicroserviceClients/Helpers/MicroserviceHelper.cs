using System.Net.Http;

namespace Kadena2.MicroserviceClients.Helpers
{
    public class MicroserviceHelper
    {
        public void AddHeader(HttpClient httpClient, string headerName, string headerValue)
        {
            httpClient.DefaultRequestHeaders.Add(headerName, headerValue);
        }
    }
}
