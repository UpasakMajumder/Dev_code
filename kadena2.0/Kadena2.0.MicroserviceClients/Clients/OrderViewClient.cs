using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Net.Http;
using System.Threading.Tasks;
using Kadena.Dto.ViewOrder.MicroserviceRequests;
using Kadena.Dto.General;

namespace Kadena2.MicroserviceClients.Clients
{
    public class OrderViewClient : ClientBase, IOrderViewClient
    {
        public async Task<BaseResponseDTO<GetOrderByOrderIdResponseDTO>> GetOrderByOrderId(string serviceEndpoint, string orderId)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{serviceEndpoint.TrimEnd('/')}/api/order/{orderId}";
                var content = CreateRequestContent(orderId);
                var response = await httpClient.GetAsync(serviceEndpoint);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return await ReadResponseJson<BaseResponseDTO<GetOrderByOrderIdResponseDTO>>(response);
                }
                else
                {
                    return new BaseResponseDTO<GetOrderByOrderIdResponseDTO>()
                    {
                        Success = false,
                        Payload = null,
                        ErrorMessage = $"HTTP error - {response.StatusCode}"
                    };
                }
            }
        }
    }
}

