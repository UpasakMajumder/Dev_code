using Kadena.Dto.General;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Net.Http;
using System.Threading.Tasks;
using Kadena.Dto.Order;

namespace Kadena2.MicroserviceClients.Clients
{
    public class OrderServiceClient : ClientBase, IOrderServiceClient
    {
        public async Task<AwsResponseMessage<OrderListDto>> GetOrders(string serviceEndpoint, string siteName, int pageNumber, int quantity)
        {
            var url = $"{serviceEndpoint}?siteName={siteName}&pageNumber={pageNumber}&quantity={quantity}";
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(serviceEndpoint))
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return await ReadResponseJson<AwsResponseMessage<OrderListDto>>(response);
                    }
                    else
                    {
                        return CreateErrorResponse<OrderListDto>($"HTTP error - {response.StatusCode}");
                    }
                }
            }
        }

        public async Task<AwsResponseMessage<string>> SubmitOrder(string serviceEndpoint, OrderDTO orderData)
        {
            using (var httpClient = new HttpClient())
            {
                var content = CreateRequestContent(orderData);
                var response = await httpClient.PostAsync(serviceEndpoint, content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return await ReadResponseJson<AwsResponseMessage<string>>(response);
                }
                else
                {
                    return CreateErrorResponse<string>($"HTTP error - {response.StatusCode}");
                }
            }
        }

        private AwsResponseMessage<T> CreateErrorResponse<T>(string errorMessage)
        {
            return new AwsResponseMessage<T>()
            {
                Success = false,
                Payload = default(T),
                Error = new ErrorMessage
                {
                    Message = errorMessage
                }
            };
        }
    }
}

