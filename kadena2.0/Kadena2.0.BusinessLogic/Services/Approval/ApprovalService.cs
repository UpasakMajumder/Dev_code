using Kadena.BusinessLogic.Contracts.Approval;
using Kadena.Dto.Approval.MicroserviceRequests;
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
        private readonly IApproverService approvers;
        private readonly IApprovalServiceClient approvalClient;
        private readonly IKenticoLogger log;
        private readonly IKenticoOrderProvider kenticoOrderProvider;
        private readonly IKenticoResourceService kenticoResource;
        private readonly IOrderViewClient orderService;
        private readonly IKenticoSkuProvider skuProvider;

        public ApprovalService(IApproverService approvers,
            IApprovalServiceClient approvalClient,
            IKenticoLogger log,
            IKenticoOrderProvider kenticoOrderProvider,
            IKenticoResourceService kenticoResource,
            IOrderViewClient orderService,
            IKenticoSkuProvider skuProvider)
        {
            this.approvers = approvers ?? throw new ArgumentNullException(nameof(approvers));
            this.approvalClient = approvalClient ?? throw new ArgumentNullException(nameof(approvalClient));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.kenticoOrderProvider = kenticoOrderProvider ?? throw new ArgumentNullException(nameof(kenticoOrderProvider));
            this.kenticoResource = kenticoResource ?? throw new ArgumentNullException(nameof(kenticoResource));
            this.orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            this.skuProvider = skuProvider ?? throw new ArgumentNullException(nameof(skuProvider));
        }

        public async Task<ApprovalResult> ApproveOrder(string orderId, int customerId, string customerName, string note = "")
        {
            await CallApprovalService(orderId, customerId, customerName, note, ApprovalState.Approved);
            return new ApprovalResult
            {
                Title = kenticoResource.GetResourceString("Kadena.Order.Approve.Success.ToastTitle"),
                Text = kenticoResource.GetResourceString("Kadena.Order.Approve.Success.ToastMessage"),
                NewStatus = kenticoOrderProvider.MapOrderStatus(OrderStatus.Approved.GetDisplayName())
            };
        }

        public async Task<ApprovalResult> RejectOrder(string orderId, int customerId, string customerName, string rejectionNote = "")
        {
            var order = await orderService.GetOrderByOrderId(orderId);
            if (!order.Success)
            {
                throw new ApprovalServiceException(order.ErrorMessages);
            }

            await CallApprovalService(orderId, customerId, customerName, rejectionNote, ApprovalState.ApprovalRejected);

            order.Payload.Items.ForEach(i => skuProvider.UpdateAvailableQuantity(i.SkuId, i.Quantity));

            return new ApprovalResult
            {
                Title = kenticoResource.GetResourceString("Kadena.Order.Reject.Success.ToastTitle"),
                Text = kenticoResource.GetResourceString("Kadena.Order.Reject.Success.ToastMessage"),
                NewStatus = kenticoOrderProvider.MapOrderStatus(OrderStatus.ApprovalRejected.GetDisplayName())
            };
        }

        async Task CallApprovalService(string orderId, int customerId, string customerName, string note, ApprovalState approvalState)
        {
            approvers.CheckIsCustomersApprover(customerId, customerName);
            var approveRequest = GetApprovalData(orderId, customerId, customerName, approvalState, note);

            var microserviceResult = await approvalClient.Approval(approveRequest).ConfigureAwait(false);

            if (!microserviceResult.Success)
            {
                var message = $"Error processing order '{approveRequest?.OrderId}' - Failed to call approval microservice. {microserviceResult.ErrorMessages}";
                log.LogError(approvalState.GetDisplayName(), message);
                throw new ApprovalServiceException(message);
            }

            if (microserviceResult.Payload != approvalState.ToString())
            {
                var message = $"Error processing order '{approveRequest?.OrderId}' - Approval microservice returned unexpected state {microserviceResult.Payload}";
                log.LogError(approvalState.GetDisplayName(), message);
                throw new ApprovalServiceException(message);
            }

            var noteLog = string.IsNullOrEmpty(note) ? null : $"Approver's note: {note}";

            log.LogInfo(approvalState.GetDisplayName(), "Info", $"Order '{approveRequest.OrderId}' successfully processed, approval status : {microserviceResult.Payload}. {noteLog}");
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
