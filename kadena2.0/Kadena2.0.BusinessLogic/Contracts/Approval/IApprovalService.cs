using Kadena.Dto.Approval.Responses;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts.Approval
{
    /// <summary>
    /// Manages approval flow operations
    /// </summary>
    public interface IApprovalService
    {
        Task<ApprovalResultDto> ApproveOrder(string orderId, int customerId, string customerName);
        Task<ApprovalResultDto> RejectOrder(string orderId, int customerId, string customerName, string rejectionNote = "");
    }
}
