using Kadena.BusinessLogic.Contracts.Approval;
using Kadena.Dto.Approval.MicroserviceRequests;
using Kadena.Models.Membership;
using Kadena.Models.SiteSettings.Permissions;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Approval
{
    public class ApproverService : IApproverService
    {
        private readonly IKenticoPermissionsProvider permissions;
        private readonly IApprovalServiceClient approvalService;
        private readonly IKenticoUserProvider users;
        private readonly IKenticoCustomerProvider customers;
        private readonly IKenticoLogger log;

        public ApproverService(IKenticoPermissionsProvider permissions, 
                               IApprovalServiceClient approvalService, 
                               IKenticoUserProvider users, 
                               IKenticoCustomerProvider customers,
                               IKenticoLogger log)
        {
            this.permissions = permissions ?? throw new ArgumentNullException(nameof(permissions));
            this.approvalService = approvalService ?? throw new ArgumentNullException(nameof(approvalService));
            this.users = users ?? throw new ArgumentNullException(nameof(users));
            this.customers = customers ?? throw new ArgumentNullException(nameof(customers));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public IEnumerable<User> GetApprovers(int siteId)
        {
            return permissions.GetUsersWithPermission(ModulePermissions.KadenaOrdersModule,
                                                      ModulePermissions.KadenaOrdersModule.ApproveOrders,
                                                      siteId);
        }

        public async Task<bool> ApproveOrder(string orderId, int customerId, string customerName)
        {
            CheckIsCustomersApprover(customerId);
            var approveRequest = GetApprovalData(orderId, customerId, customerName, ApprovalState.Approved);
            var microserviceResult = await approvalService.Approval(approveRequest).ConfigureAwait(false);
            var success = !microserviceResult.Success || !microserviceResult.Payload;
            if (!success)
            {
                log.LogError("Approval", $"Error approving order {orderId} - Failed to call approval microservice. {microserviceResult.ErrorMessages}");
            }
            return success;
        }

        public async Task<bool> RejectOrder(string orderId, int customerId, string customerName, string rejectionNote = "")
        {
            CheckIsCustomersApprover(customerId);
            var approveRequest = GetApprovalData(orderId, customerId, customerName, ApprovalState.Rejected, rejectionNote);
            var microserviceResult = await approvalService.Approval(approveRequest).ConfigureAwait(false);
            var success = !microserviceResult.Success || !microserviceResult.Payload;
            if (!success)
            {
                log.LogError("Approval", $"Error rejecting order {orderId} - Failed to call approval microservice. {microserviceResult.ErrorMessages}");
            }
            return success;
        }

        void CheckIsCustomersApprover(int customerId)
        {
            var currentUserId = users.GetCurrentUser()?.UserId ?? 0;

            if (!IsCustomersApprover(currentUserId, customerId))
            {
                throw new Exception($"Current user is not an approver of customer {customerId}");
            }
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

        private ApprovalRequestDto GetApprovalData(string orderId, int customerId, string customerName, int state, string rejectionNote = "")
        {
            return new ApprovalRequestDto
            {
                ApproversCount = 1,
                OrderId = orderId,
                Approvals = new ApprovalUnitDto[]
                {
                    new ApprovalUnitDto
                    {
                        Customer = new CustomerDto
                        {
                            Id = customerId,
                            Name = customerName,
                        },
                        Note = rejectionNote,
                        State = state
                    }
                }
            };
        }

    }
}
