using Kadena.Dto.General;
using Kadena.Dto.ViewOrder.MicroserviceResponses;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IOrderViewClient
    {
        Task<BaseResponseDTO<GetOrderByOrderIdResponseDTO>> GetOrderByOrderId(string serviceEndpoint, string orderId);
    }
}
