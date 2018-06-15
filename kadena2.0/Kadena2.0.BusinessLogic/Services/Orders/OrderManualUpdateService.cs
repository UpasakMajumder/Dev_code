using Kadena.BusinessLogic.Contracts.Approval;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Dto.OrderManualUpdate.MicroserviceRequests;
using Kadena.Models.Orders;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Linq;
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

            // TODO :
            // check if send price to ERP
            // check minmax
            // pricing model and update price respectively
            // inventory count, mabe involve SellIfNotAvailable
            // if payd by card, allow only decrease

            var totalPrice = 0.0m;
            var totalTax = 0.0m;
            var totalShipping = 0.0m;

            var requestDto = new OrderManualUpdateRequestDto
            {
                OrderId = request.OrderId,
                Items = request.Items.Select(i => new ItemUpdateDto
                {
                    LineNumber = i.LineNumber,
                    UnitCount = i.Quantity
                }).ToList(),
                TotalPrice = totalPrice,
                TotalTax = totalTax,
                TotalShipping = totalShipping
            };

            /*updateService.UpdateOrder()*/

        }
    }
}
