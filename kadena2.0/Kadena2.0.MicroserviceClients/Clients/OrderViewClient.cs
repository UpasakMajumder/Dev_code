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
                var response = await httpClient.GetAsync(url);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return await ReadResponseJson<AwsResponseMessage<GetOrderByOrderIdResponseDTO>>(response);
                }
                else
                {
                    return new AwsResponseMessage<GetOrderByOrderIdResponseDTO>()
                    {
                        Success = false,
                        Payload = null,
                        ErrorMessages = $"HTTP error - {response.StatusCode}"
                    };
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
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return await ReadResponseJson<AwsResponseMessage<OrderListDto>>(response);
                    }
                    else
                    {
                        return new AwsResponseMessage<OrderListDto>()
                        {
                            Success = false,
                            Payload = default(OrderListDto),
                            Error = new ErrorMessage
                            {
                                Message = $"HTTP error - {response.StatusCode}"
                            }
                        };
                    }
                }
            }
        }
    }
}

