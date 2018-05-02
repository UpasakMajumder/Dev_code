using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts.Approval
{
    /// <summary>
    /// Manages approval flow operations
    /// </summary>
    public interface IApprovalService
    {
        Task<bool> ApproveOrder(string orderId, int customerId, string customerName);
        Task<bool> RejectOrder(string orderId, int customerId, string customerName, string rejectionNote = "");
    }
}
