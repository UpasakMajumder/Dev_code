using Kadena.Dto.General;
using Kadena.Dto.Order.Failed;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IOrderResubmitClient
    {
        Task<BaseResponseDto<ResubmitOrderResponseDto>> Resubmit(ResubmitOrderRequestDto requestDto);
    }
}
