using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena2.MicroserviceClients.MicroserviceResponses;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IOrderSubmitClient
    {
        Task<SubmitOrderServiceResponseDto> SubmitOrder(string serviceEndpoint, OrderDTO orderData); // TODO refactor to common microservice response
    }
}
