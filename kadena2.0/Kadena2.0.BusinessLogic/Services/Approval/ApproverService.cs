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
        private readonly IKenticoUserProvider users;
        private readonly IKenticoSettingsProvider settingsProvider;
        private readonly IKenticoSiteProvider siteProvider;

        public ApproverService(IKenticoPermissionsProvider permissions, 
                               IKenticoCustomerProvider customers,
                               IKenticoUserProvider users,
                               IKenticoSettingsProvider settingsProvider,
                               IKenticoSiteProvider siteProvider)
        {
            this.permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
            this.customers = customers ?? throw new ArgumentNullException(nameof(customers));
            this.users = users ?? throw new ArgumentNullException(nameof(users));
            this.settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
            this.siteProvider = siteProvider ?? throw new ArgumentNullException(nameof(siteProvider));
        }

        public IEnumerable<User> GetApprovers(int siteId)
        {
            return permissions.GetUsersWithPermission(ModulePermissions.KadenaOrdersModule,
                                                      ModulePermissions.KadenaOrdersModule.ApproveOrders,
                                                      siteId);
        }

        public bool IsApprover(int userId) =>
            permissions.UserHasPermission(
                userId,
                ModulePermissions.KadenaOrdersModule,
                ModulePermissions.KadenaOrdersModule.ApproveOrders);

        bool IsEditor(int userId) =>
            permissions.UserHasPermission(
                userId,
                ModulePermissions.KadenaOrdersModule,
                ModulePermissions.KadenaOrdersModule.EditOrdersInApproval);

        public bool IsCustomersApprover(int approverUserId, int customerId)
        {
            if (approverUserId == 0 || customerId == 0)
            {
                return false;
            }

            if(!IsApprover(approverUserId))
            {
                return false;
            }

            var customer = customers.GetCustomer(customerId);
            if (customer == null)
            {
                return false;
            }

            var customersApproverUserId = customer.ApproverUserId;

            if (customersApproverUserId == 0)
            {
                var site = siteProvider.GetKenticoSite(customer.SiteId);
               
                // check if there is a default approver for this site
               //var defaultApproverId = 
                
            }

            return customersApproverUserId == approverUserId;
        }

        public void CheckIsCustomersApprover(int customerId, string customerName)
        {
            var currentUser = users.GetCurrentUser();

            if ((currentUser == null) || !IsCustomersApprover(currentUser.UserId, customerId))
            {
                throw new Exception($"Current User is not an approver of customer '{customerName}' (Id={customerId})");
            }
        }

        public void CheckIsCustomersEditor(int customerId)
        {
            var currentUser = users.GetCurrentUser();

            if ((currentUser == null) || !IsCustomersApprover(currentUser.UserId, customerId) || !IsEditor(currentUser.UserId))
            {
                throw new Exception($"Current User has no permissions to edit orders of customer with Id={customerId}");
            }
        }
    }
}
