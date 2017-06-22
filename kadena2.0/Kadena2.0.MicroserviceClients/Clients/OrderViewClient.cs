using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Net.Http;
using System.Threading.Tasks;
using Kadena.Dto.ViewOrder.MicroserviceResponses;
using Kadena.Dto.General;
using Kadena.Dto.Order;

namespace Kadena2.MicroserviceClients.Clients
{
    public class OrderViewClient : ClientBase, IOrderViewClient
    {
        public async Task<AwsResponseMessage<GetOrderByOrderIdResponseDTO>> GetOrderByOrderId(string serviceEndpoint, string orderId)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{serviceEndpoint.TrimEnd('/')}/api/order/{orderId}";
                using (var response = await httpClient.GetAsync(url))
                {
                    return await ReadResponseJson<GetOrderByOrderIdResponseDTO>(response);
                }
            }
        }

        public async Task<AwsResponseMessage<OrderListDto>> GetOrders(string serviceEndpoint, string siteName, int pageNumber, int quantity)
        {
            var url = $"{serviceEndpoint}?siteName={siteName}&pageNumber={pageNumber}&quantity={quantity}";
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    return await ReadResponseJson<OrderListDto>(response);
                }
            }
        }
    }
}

