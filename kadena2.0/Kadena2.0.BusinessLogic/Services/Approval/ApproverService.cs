using Kadena.BusinessLogic.Contracts.Approval;
using Kadena.Models.Membership;
using Kadena.Models.SiteSettings.Permissions;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Services.Approval
{
    public class ApproverService : IApproverService
    {
        private readonly IKenticoPermissionsProvider permissions;
        private readonly IKenticoCustomerProvider customers;

        public ApproverService(IKenticoPermissionsProvider permissions, 
                               IKenticoCustomerProvider customers)
        {
            this.permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
            this.customers = customers ?? throw new ArgumentNullException(nameof(customers));
        }

        public IEnumerable<User> GetApprovers(int siteId)
        {
            return permissions.GetUsersWithPermission(ModulePermissions.KadenaOrdersModule,
                                                      ModulePermissions.KadenaOrdersModule.ApproveOrders,
                                                      siteId);
        }

        public bool IsCustomersApprover(int approverUserId, int customerId)
        {
            if (approverUserId == 0 || customerId == 0)
            {
                return false;
            }

            if(!permissions.UserHasPermission(approverUserId, ModulePermissions.KadenaOrdersModule, ModulePermissions.KadenaOrdersModule.ApproveOrders))
            {
                return false;
            }

            var customersApproverUserId = customers.GetCustomer(customerId)?.ApproverUserId ?? 0;

            return customersApproverUserId != 0 && 
                   customersApproverUserId == approverUserId;
        }
    }
}
