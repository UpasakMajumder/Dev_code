using Kadena.BusinessLogic.Contracts.Approval;
using Kadena.Dto.Approval.MicroserviceRequests;
using Kadena.Dto.Approval.MicroserviceResponses;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Approval
{
    public class ApprovalService : IApprovalService
    {
        private readonly IApproverService approvers;
        private readonly IApprovalServiceClient approvalClient;
        private readonly IKenticoUserProvider users;
        private readonly IKenticoLogger log;

        public ApprovalService(IApproverService approverService,
                               IApprovalServiceClient approvalMicroservice, 
                               IKenticoUserProvider users, 
                               IKenticoLogger log)
        {
            this.approvers = approverService ?? throw new ArgumentNullException(nameof(approverService));
            this.approvalClient = approvalMicroservice ?? throw new ArgumentNullException(nameof(approvalMicroservice));
            this.users = users ?? throw new ArgumentNullException(nameof(users));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public async Task<bool> ApproveOrder(string orderId, int customerId, string customerName)
        {
            CheckIsCustomersApprover(customerId, customerName);
            var approveRequest = GetApprovalData(orderId, customerId, customerName, ApprovalState.Approved);
            return await CallApprovalService(approveRequest, "ApproveOrder", ApprovalResponseDto.Approved);
        }

        public async Task<bool> RejectOrder(string orderId, int customerId, string customerName, string rejectionNote = "")
        {
            CheckIsCustomersApprover(customerId, customerName);
            var approveRequest = GetApprovalData(orderId, customerId, customerName, ApprovalState.Rejected, rejectionNote);
            return await CallApprovalService(approveRequest, "RejectOrder", ApprovalResponseDto.Rejected);
        }

        async Task<bool> CallApprovalService(ApprovalRequestDto request, string operationName, string expectedResult)
        {
            var microserviceResult = await approvalClient.Approval(request).ConfigureAwait(false);

            if (!microserviceResult.Success)
            {
                log.LogError(operationName, $"Error processing order '{request?.OrderId}' - Failed to call approval microservice. {microserviceResult.ErrorMessages}");
                return false;
            }

            if (microserviceResult.Payload != expectedResult)
            {
                log.LogError(operationName, $"Error processing order '{request?.OrderId}' - Approval microservice returned unexpected state {microserviceResult.Payload}");
                return false;
            }

            var note = request.Approvals?[0]?.Note;
            var noteLog = string.IsNullOrEmpty(note) ? null : $"Approver's note: {note}";

            log.LogInfo(operationName,"Info", $"Order '{request.OrderId}' successfully processed, approval status : {microserviceResult.Payload}. {noteLog}");

            return true;
        }


        void CheckIsCustomersApprover(int customerId, string customerName)
        {
            var currentUser = users.GetCurrentUser();

            if ((currentUser == null) || !approvers.IsCustomersApprover(currentUser.UserId, customerId))
            {
                throw new Exception($"Current User is not an approver of customer '{customerName}' (Id={customerId})");
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
