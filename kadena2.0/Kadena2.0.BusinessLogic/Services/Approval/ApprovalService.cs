using Kadena.BusinessLogic.Contracts.Approval;
using Kadena.Dto.Approval.MicroserviceRequests;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Approval
{
    public class ApprovalService : IApprovalService
    {
        private readonly IApproverService approverService;
        private readonly IApprovalServiceClient approvalMicroservice;
        private readonly IKenticoUserProvider users;
        private readonly IKenticoLogger log;

        public ApprovalService(IApproverService approverService,
                               IApprovalServiceClient approvalMicroservice, 
                               IKenticoUserProvider users, 
                               IKenticoLogger log)
        {
            this.approverService = approverService ?? throw new ArgumentNullException(nameof(approverService));
            this.approvalMicroservice = approvalMicroservice ?? throw new ArgumentNullException(nameof(approvalMicroservice));
            this.users = users ?? throw new ArgumentNullException(nameof(users));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public async Task<bool> ApproveOrder(string orderId, int customerId, string customerName)
        {
            CheckIsCustomersApprover(customerId, customerName);
            var approveRequest = GetApprovalData(orderId, customerId, customerName, ApprovalState.Approved);
            var microserviceResult = await approvalMicroservice.Approval(approveRequest).ConfigureAwait(false);
            var success = microserviceResult.Success && microserviceResult.Payload;
            if (!success)
            {
                log.LogError("Approval", $"Error approving order {orderId} - Failed to call approval microservice. {microserviceResult.ErrorMessages}");
            }
            return success;
        }

        public async Task<bool> RejectOrder(string orderId, int customerId, string customerName, string rejectionNote = "")
        {
            CheckIsCustomersApprover(customerId, customerName);
            var approveRequest = GetApprovalData(orderId, customerId, customerName, ApprovalState.Rejected, rejectionNote);
            var microserviceResult = await approvalMicroservice.Approval(approveRequest).ConfigureAwait(false);
            var success = microserviceResult.Success && microserviceResult.Payload;
            if (!success)
            {
                log.LogError("Approval", $"Error rejecting order {orderId} - Failed to call approval microservice. {microserviceResult.ErrorMessages}");
            }
            return success;
        }

        void CheckIsCustomersApprover(int customerId, string customerName)
        {
            var currentUser = users.GetCurrentUser();

            if ((currentUser == null) || !approverService.IsCustomersApprover(currentUser.UserId, customerId))
            {
                throw new Exception($"Current User is not an approver of customer {customerId}");
            }
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
