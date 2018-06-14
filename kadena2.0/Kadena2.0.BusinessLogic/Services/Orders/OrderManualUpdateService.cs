using Kadena.BusinessLogic.Contracts.Approval;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Dto.OrderManualUpdate.MicroserviceRequests;
using Kadena.Models.Orders;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class OrderManualUpdateService : IOrderManualUpdateService
    {
        private readonly IOrderManualUpdateClient updateService;
        private readonly IApproverService approvers;

        public OrderManualUpdateService(IOrderManualUpdateClient updateService, IApproverService approvers)
        {
            this.updateService = updateService ?? throw new ArgumentNullException(nameof(updateService));
            this.approvers = approvers ?? throw new ArgumentNullException(nameof(approvers));
        }

        public async Task UpdateOrder(OrderUpdate request)
        {
            approvers.CheckIsCustomersEditor(request.CustomerId);

            var requestDto = new OrderManualUpdateRequestDto
            {

            };


            /*updateService.UpdateOrder()*/

        }
    }
}
