using Kadena.Dto.General;
using Kadena.Dto.Payment.CreditCard.MicroserviceRequests;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public class CreditCardManagerClient : ClientBase, ICreditCardManagerClient
    {
        public async Task<BaseResponseDto<object>> CreateCustomerContainer(string serviceEndpoint, CreateCustomerContainerRequestDto request)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{serviceEndpoint.TrimEnd('/')}/api/Customer/";
                var content = CreateRequestContent(request);
                using (var response = await httpClient.PostAsync(url, content).ConfigureAwait(false))
                {
                    return await ReadResponseJson<object>(response).ConfigureAwait(false);
                }
            }
        }
    }
}
