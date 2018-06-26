using Kadena.Models.Membership;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Contracts.Approval
{
    /// <summary>
    /// Manages approver Users and their relationship to Customers
    /// </summary>
    public interface IApproverService
    {
        IEnumerable<User> GetApprovers(int siteId);
        bool IsCustomersApprover(int approverUserId, int customerId);
        bool IsApprover(int userId);
        /// <summary>
        /// Checks if current user is approver of given customer and has necessary permissions
        /// </summary>
        void CheckIsCustomersApprover(int customerId, string customerName);
        /// <summary>
        /// Checks if current user is approver and editor of given customer and has necessary permissions
        /// </summary>
        void CheckIsCustomersEditor(int customerId);
    }
}
