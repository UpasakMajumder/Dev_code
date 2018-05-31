using Kadena.BusinessLogic.Contracts.Approval;
using Kadena.Dto.Approval.MicroserviceRequests;
using Kadena.Dto.Approval.MicroserviceResponses;
using Kadena.Helpers;
using Kadena.Models.Approval;
using Kadena.Models.Orders;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using System;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Approval
{
    public class ApprovalService : IApprovalService
    {
        struct ApprovalServiceCallResult
        {
            public bool Success;
            public string Error;
            public string NewStatus;
        }

        private readonly IApproverService approvers;
        private readonly IApprovalServiceClient approvalClient;
        private readonly IKenticoUserProvider users;
        private readonly IKenticoLogger log;
        private readonly IKenticoOrderProvider kenticoOrderProvider;
        private readonly IKenticoResourceService kenticoResource;

        public ApprovalService(IApproverService approvers,
                               IApprovalServiceClient approvalClient,
                               IKenticoUserProvider users,
                               IKenticoLogger log,
                               IKenticoOrderProvider kenticoOrderProvider,
                               IKenticoResourceService kenticoResource)
        {
            this.approvers = approvers ?? throw new ArgumentNullException(nameof(approvers));
            this.approvalClient = approvalClient ?? throw new ArgumentNullException(nameof(approvalClient));
            this.users = users ?? throw new ArgumentNullException(nameof(users));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.kenticoOrderProvider = kenticoOrderProvider ?? throw new ArgumentNullException(nameof(kenticoOrderProvider));
            this.kenticoResource = kenticoResource ?? throw new ArgumentNullException(nameof(kenticoResource));
        }

        public async Task<ApprovalResult> ApproveOrder(string orderId, int customerId, string customerName, string note = "")
        {
            CheckIsCustomersApprover(customerId, customerName);
            var approveRequest = GetApprovalData(orderId, customerId, customerName, ApprovalState.Approved, note);
            var response = await CallApprovalService(approveRequest, "ApproveOrder", ApprovalResponseDto.Approved);
            if (response.Success)
            {
                return new ApprovalResult
                {
                    Title = kenticoResource.GetResourceString("Kadena.Order.Approve.Success.ToastTitle"),
                    Text = kenticoResource.GetResourceString("Kadena.Order.Approve.Success.ToastMessage"),
                    NewStatus = kenticoOrderProvider.MapOrderStatus(OrderStatus.Approved.GetDisplayName())
                };
            }
            else
            {
                throw new ApprovalServiceException(response.Error);
            }
        }

        public async Task<ApprovalResult> RejectOrder(string orderId, int customerId, string customerName, string rejectionNote = "")
        {
            CheckIsCustomersApprover(customerId, customerName);
            var approveRequest = GetApprovalData(orderId, customerId, customerName, ApprovalState.Rejected, rejectionNote);
            var response = await CallApprovalService(approveRequest, "RejectOrder", ApprovalResponseDto.Rejected);
            if (response.Success)
            {
                return new ApprovalResult
                {
                    Title = kenticoResource.GetResourceString("Kadena.Order.Reject.Success.ToastTitle"),
                    Text = kenticoResource.GetResourceString("Kadena.Order.Reject.Success.ToastMessage"),
                    NewStatus = kenticoOrderProvider.MapOrderStatus(OrderStatus.Rejected.GetDisplayName())
                };
            }
            else
            {
                throw new ApprovalServiceException(response.Error);
            }
        }

        async Task<ApprovalServiceCallResult> CallApprovalService(ApprovalRequestDto request, string operationName, string expectedResult)
        {
            var microserviceResult = await approvalClient.Approval(request).ConfigureAwait(false);

            if (!microserviceResult.Success)
            {
                var message = $"Error processing order '{request?.OrderId}' - Failed to call approval microservice. {microserviceResult.ErrorMessages}";
                log.LogError(operationName, message);
                return new ApprovalServiceCallResult { Success = false, Error = message };
            }

            if (microserviceResult.Payload != expectedResult)
            {
                var message = $"Error processing order '{request?.OrderId}' - Approval microservice returned unexpected state {microserviceResult.Payload}";
                log.LogError(operationName, message);
                return new ApprovalServiceCallResult { Success = false, Error = message };
            }

            var note = request.Approvals?[0]?.Note;
            var noteLog = string.IsNullOrEmpty(note) ? null : $"Approver's note: {note}";

            log.LogInfo(operationName, "Info", $"Order '{request.OrderId}' successfully processed, approval status : {microserviceResult.Payload}. {noteLog}");

            return new ApprovalServiceCallResult { Success = true, NewStatus = microserviceResult.Payload };
        }

        void CheckIsCustomersApprover(int customerId, string customerName)
        {
            var currentUser = users.GetCurrentUser();

            if ((currentUser == null) || !approvers.IsCustomersApprover(currentUser.UserId, customerId))
            {
                throw new Exception($"Current User is not an approver of customer '{customerName}' (Id={customerId})");
            }
        }

        private ApprovalRequestDto GetApprovalData(string orderId, int customerId, string customerName, ApprovalState state, string rejectionNote)
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
                        State = (int)state
                    }
                }
            };
        }
    }
}
