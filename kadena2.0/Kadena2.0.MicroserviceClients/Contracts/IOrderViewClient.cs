using Kadena.Dto.General;
using Kadena.Dto.Order;
using Kadena.Dto.ViewOrder.MicroserviceResponses;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IOrderViewClient
    {
        Task<BaseResponseDto<GetOrderByOrderIdResponseDTO>> GetOrderByOrderId(string orderId);
        Task<BaseResponseDto<OrderListDto>> GetOrders(string siteName, int pageNumber, int quantity);
        Task<BaseResponseDto<OrderListDto>> GetOrders(int customerId, int pageNumber, int quantity);
        Task<BaseResponseDto<OrderListDto>> GetOrders(string siteName, int pageNumber, int quantity, int campaignID, string orderType);
        Task<BaseResponseDto<OrderListDto>> GetOrders(int customerId, int pageNumber, int quantity, int campaignID, string orderType);
    }
}
