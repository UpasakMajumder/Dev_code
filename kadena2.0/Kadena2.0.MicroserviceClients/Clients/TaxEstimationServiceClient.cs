using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.MicroserviceRequests;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public class TaxEstimationServiceClient : ClientBase, ITaxEstimationServiceClient
    {
        public async Task<BaseResponseDto<decimal>> CalculateTax(string serviceEndpoint, TaxCalculatorRequestDto request)
        {
            return await Post<decimal>(serviceEndpoint, request).ConfigureAwait(false);
        }
    }
}
