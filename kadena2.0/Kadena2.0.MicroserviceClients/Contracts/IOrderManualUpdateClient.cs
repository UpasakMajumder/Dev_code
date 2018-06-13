using Kadena.Dto.General;
using Kadena.Dto.OrderManualUpdate.MicroserviceRequests;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IOrderManualUpdateClient
    {
        Task<BaseResponseDto<object>> UpdateOrder(OrderManualUpdateRequestDto request);
    }
}
