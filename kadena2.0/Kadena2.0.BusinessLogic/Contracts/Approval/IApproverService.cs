using Kadena.Models.Membership;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts.Approval
{
    public interface IApproverService
    {
        IEnumerable<User> GetApprovers(int siteId);
        bool IsCustomersApprover(int approverUserId, int customerId);
        Task<bool> ApproveOrder(string orderId, int customerId, string customerName);
        Task<bool> RejectOrder(string orderId, int customerId, string customerName, string rejectionNote = "");
    }
}
