using Kadena.Dto.General;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Net.Http;
using System.Threading.Tasks;
using Kadena.Dto.Order;

namespace Kadena2.MicroserviceClients.Clients
{
    public class OrderSubmitClient : ClientBase, IOrderSubmitClient
    {
        public async Task<BaseResponse<string>> SubmitOrder(string serviceEndpoint, OrderDTO orderData)
        {
            using (var httpClient = new HttpClient())
            {
                var content = CreateRequestContent(orderData);
                using (var response = await httpClient.PostAsync(serviceEndpoint, content))
                {
                    return await ReadResponseJson<string>(response);
                }
            }
        }
    }
}

