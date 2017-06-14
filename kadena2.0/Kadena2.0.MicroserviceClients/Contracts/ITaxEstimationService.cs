using Kadena2.MicroserviceClients.MicroserviceRequests;
using Kadena2.MicroserviceClients.MicroserviceResponses;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface ITaxEstimationService
    {
        Task<TaxCalculatorServiceResponseDto> CalculateTax(string serviceEndpoint, TaxCalculatorRequestDto request);
    }
}
