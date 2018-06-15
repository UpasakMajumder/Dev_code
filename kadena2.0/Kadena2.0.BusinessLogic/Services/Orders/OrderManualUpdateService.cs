using Kadena.BusinessLogic.Contracts.Approval;
using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Dto.OrderManualUpdate.MicroserviceRequests;
using Kadena.Models.Orders;
using Kadena.Models.Product;
using Kadena.WebAPI.KenticoProviders.Contracts;
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
        private readonly IKenticoSkuProvider skuProvider;

        public OrderManualUpdateService(IOrderManualUpdateClient updateService, IApproverService approvers, IKenticoSkuProvider skuProvider)
        {
            this.updateService = updateService ?? throw new ArgumentNullException(nameof(updateService));
            this.approvers = approvers ?? throw new ArgumentNullException(nameof(approvers));
            this.skuProvider = skuProvider ?? throw new ArgumentNullException(nameof(skuProvider));
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

            var kenticoSkus = skuProvider.GetSKUsByNumbers(request.Items.Select(i => i.SKUNumber).ToArray());

            var changedItems = request.Items.Select(i => CreateChangedItem(i)).ToList();

            var requestDto = new OrderManualUpdateRequestDto
            {
                OrderId = request.OrderId,
                Items = changedItems,
                TotalPrice = totalPrice,
                TotalTax = totalTax,
                TotalShipping = totalShipping
            };

            /*updateService.UpdateOrder()*/
        }

        ItemUpdateDto CreateChangedItem(OrderItemUpdate item)
        {


            return new ItemUpdateDto
            {
                LineNumber = item.LineNumber,
                UnitCount = item.Quantity
            };
        }
    }
}
