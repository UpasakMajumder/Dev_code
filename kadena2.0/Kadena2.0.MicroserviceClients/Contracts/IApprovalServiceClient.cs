using Kadena.Dto.Approval.MicroserviceRequests;
using Kadena.Dto.General;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IApprovalServiceClient
    {
        Task<BaseResponseDto<bool>> Approval(ApprovalRequestDto approval);
    }
}
