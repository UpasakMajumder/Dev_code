using Kadena.BusinessLogic.Contracts.Approval;
using Kadena.Models.Membership;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Services.Approval
{
    public class ApproverService  : IApproverService
    {
        public string ApproversRoleName => "Approvers";

        private readonly IKenticoRoleProvider roles;

        public ApproverService(IKenticoRoleProvider roles)
        {
            this.roles = roles ?? throw new ArgumentNullException(nameof(roles));
        }

        public IEnumerable<User> GetApprovers(int siteId)
        {
            return roles.GetRoleUsers(ApproversRoleName, siteId);
        }
    }
}
