using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Net.Http;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena.Dto.CreditCard.MicroserviceRequests;
using Kadena.Dto.CreditCard.MicroserviceResponses;

namespace Kadena2.MicroserviceClients.Clients
{
    public class PaymentServiceClient : ClientBase, IPaymentServiceClient
    {
        public async Task<BaseResponseDto<AuthorizeAmountResponseDto>> AuthorizeAmount(string serviceEndpoint, AuthorizeAmountRequestDto request)
        {
            using (var httpClient = new HttpClient())
            {
                var url = $"{serviceEndpoint.TrimEnd('/')}/api/Payment/PutHold";
                var content = CreateRequestContent(request);
                using (var response = await httpClient.PostAsync(url, content).ConfigureAwait(false))
                {
                    return await ReadResponseJson<AuthorizeAmountResponseDto>(response).ConfigureAwait(false);
                }
            }
        }
    }
}

