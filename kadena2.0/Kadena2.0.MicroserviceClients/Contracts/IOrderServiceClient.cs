using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena2.MicroserviceClients.MicroserviceResponses;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IOrderServiceClient
    {
        Task<SubmitOrderServiceResponseDto> SubmitOrder(string serviceEndpoint, OrderDTO orderData);
    }
}
