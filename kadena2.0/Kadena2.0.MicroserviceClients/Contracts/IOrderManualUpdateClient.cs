using Kadena.Dto.General;
using Kadena.Dto.OrderManualUpdate.MicroserviceRequests;
using Kadena.Dto.ViewOrder.MicroserviceResponses.History;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IOrderManualUpdateClient
    {
        Task<BaseResponseDto<object>> UpdateOrder(OrderManualUpdateRequestDto request);
        Task<BaseResponseDto<List<OrderHistoryUpdateDto>>> Get(string orderId);
    }
}
