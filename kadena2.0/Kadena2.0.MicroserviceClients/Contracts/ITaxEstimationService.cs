using Kadena2.MicroserviceClients.Requests;
using Kadena2.MicroserviceClients.Responses;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface ITaxEstimationService
    {
        Task<TaxCalculatorServiceResponseDto> CalculateTax(string serviceEndpoint, TaxCalculatorRequestDto request);
    }
}
