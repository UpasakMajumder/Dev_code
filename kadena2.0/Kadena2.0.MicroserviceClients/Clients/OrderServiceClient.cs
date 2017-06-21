using Kadena.Dto.General;
using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public class OrderServiceClient : ClientBase, IOrderServiceClient
    {
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
                    return CreateErrorResponse($"HTTP error - {response.StatusCode}");
                }
            }
        }

        private AwsResponseMessage<string> CreateErrorResponse(string errorMessage)
        {
            return new AwsResponseMessage<string>()
            {
                Success = false,
                Payload = null,
                Error = new ErrorMessage
                {
                    Message = errorMessage
                }
            };
        }
    }
}

