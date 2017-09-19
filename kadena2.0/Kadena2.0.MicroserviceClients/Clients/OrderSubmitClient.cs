using Kadena.Dto.General;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace Kadena2.MicroserviceClients.Clients
{
    public class OrderSubmitClient : ClientBase, IOrderSubmitClient
    {
        public async Task<BaseResponseDto<string>> FinishOrder(string serviceEndpoint, string orderNumber)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromSeconds(60);
                var content = new StringContent(orderNumber);
                var request = new HttpRequestMessage(new HttpMethod("PATCH"), serviceEndpoint) { Content = content };
                using (var response = await httpClient.SendAsync(request))
                {
                    return await ReadResponseJson<string>(response);
                }
            }
        }

        public async Task<BaseResponseDto<string>> SubmitOrder(string serviceEndpoint, OrderDTO orderData)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromSeconds(60);
                var content = CreateRequestContent(orderData);
                using (var response = await httpClient.PostAsync(serviceEndpoint, content))
                {
                    return await ReadResponseJson<string>(response);
                }
            }
        }
    }
}

