using Kadena.Models.Membership;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Contracts.Approval
{
    public interface IApproverService
    {
        IEnumerable<User> GetApprovers(int siteId);
    }
}
