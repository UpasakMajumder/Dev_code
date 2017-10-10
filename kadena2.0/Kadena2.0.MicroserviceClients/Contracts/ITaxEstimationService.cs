using Kadena.Dto.General;
using Kadena2.MicroserviceClients.MicroserviceRequests;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface ITaxEstimationServiceClient
    {
        Task<BaseResponseDto<decimal>> CalculateTax(string serviceEndpoint, TaxCalculatorRequestDto request);
    }
}
