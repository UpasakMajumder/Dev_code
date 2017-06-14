using Kadena.Dto.SubmitOrder;
using Kadena2.MicroserviceClients.Responses;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IOrderServiceClient
    {
        Task<SubmitOrderServiceResponseDto> SubmitOrder(string serviceEndpoint, OrderDTO orderData);
    }
}
