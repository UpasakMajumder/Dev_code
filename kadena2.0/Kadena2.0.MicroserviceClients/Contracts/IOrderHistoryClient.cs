using Kadena.Dto.General;
using Kadena.Dto.ViewOrder.MicroserviceResponses.History;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IOrderHistoryClient
    {
        Task<BaseResponseDto<List<OrderHistoryUpdateDto>>> Get(string orderId);
    }
}
