using Kadena.Dto.General;
using Kadena.Dto.OrderManualUpdate.MicroserviceRequests;
using Kadena.Dto.ViewOrder.MicroserviceResponses.History;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kadena.Dto.OrderManualUpdate.MicroserviceRequests.UpdateShipping;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IOrderManualUpdateClient
    {
        Task<BaseResponseDto<object>> UpdateOrder(OrderManualUpdateItemsRequestDto request);
        Task<BaseResponseDto<object>> UpdateOrderPayment(OrderManualUpdatePaymentRequestDto request);
        Task<BaseResponseDto<object>> UpdateOrdersShippings(UpdateShippingsRequestDto request);
        Task<BaseResponseDto<List<OrderHistoryUpdateDto>>> Get(string orderId);
    }
}
