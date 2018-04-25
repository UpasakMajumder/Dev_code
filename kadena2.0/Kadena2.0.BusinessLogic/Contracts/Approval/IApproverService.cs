using Kadena.Models.Membership;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Contracts.Approval
{
    public interface IApproverService
    {
        string ApproversRoleName { get; }
        IEnumerable<User> GetApprovers(int siteId);
    }
}
