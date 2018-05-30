using System.Threading.Tasks;
using Kadena.Models.Approval;

namespace Kadena.BusinessLogic.Contracts.Approval
{
    /// <summary>
    /// Manages approval flow operations
    /// </summary>
    public interface IApprovalService
    {
        Task<ApprovalResult> ApproveOrder(string orderId, int customerId, string customerName);
        Task<ApprovalResult> RejectOrder(string orderId, int customerId, string customerName, string rejectionNote = "");
    }
}
