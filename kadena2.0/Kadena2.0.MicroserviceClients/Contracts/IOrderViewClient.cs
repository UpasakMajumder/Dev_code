using Kadena.Dto.General;
using Kadena.Dto.Order;
using Kadena.Dto.ViewOrder.MicroserviceResponses;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IOrderViewClient
    {
        Task<BaseResponse<GetOrderByOrderIdResponseDTO>> GetOrderByOrderId(string serviceEndpoint, string orderId);
        Task<BaseResponse<OrderListDto>> GetOrders(string serviceEndpoint, string siteName, int pageNumber, int quantity);
        Task<BaseResponse<OrderListDto>> GetOrders(string serviceEndpoint, int customerId, int pageNumber, int quantity);
    }
}
