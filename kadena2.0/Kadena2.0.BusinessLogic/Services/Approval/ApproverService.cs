using Kadena.BusinessLogic.Contracts.Approval;
using Kadena.Models.Membership;
using Kadena.Models.SiteSettings.Permissions;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Services.Approval
{
    public class ApproverService  : IApproverService
    {
        private readonly IKenticoPermissionsProvider permissions;

        public ApproverService(IKenticoPermissionsProvider permissions)
        {
            this.permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
        }

        public IEnumerable<User> GetApprovers(int siteId)
        {
            return permissions.GetUsersWithPermission(ModulePermissions.KadenaOrdersModule,
                                                      ModulePermissions.KadenaOrdersModule.ApproveOrders,
                                                      siteId);
        }
    }
}
