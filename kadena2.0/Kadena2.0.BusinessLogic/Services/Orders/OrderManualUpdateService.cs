using Kadena.BusinessLogic.Contracts;
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
        private readonly IOrderItemCheckerService orderChecker;
        private readonly IProductsService products;
        

        public OrderManualUpdateService(IOrderManualUpdateClient updateService, 
                                        IApproverService approvers, 
                                        IKenticoSkuProvider skuProvider, 
                                        IOrderItemCheckerService orderChecker,
                                        IProductsService products)
        {
            this.updateService = updateService ?? throw new ArgumentNullException(nameof(updateService));
            this.approvers = approvers ?? throw new ArgumentNullException(nameof(approvers));
            this.skuProvider = skuProvider ?? throw new ArgumentNullException(nameof(skuProvider));
            this.orderChecker = orderChecker ?? throw new ArgumentNullException(nameof(orderChecker));
            this.products = products ?? throw new ArgumentNullException(nameof(products));
        }

        public async Task UpdateOrder(OrderUpdate request)
        {
            approvers.CheckIsCustomersEditor(request.CustomerId);

            // TODO :                       
            // check if send price to ERP
            var totalPrice = 0.0m;
            var totalTax = 0.0m;
            var totalShipping = 0.0m;

            var kenticoSkus = skuProvider.GetSKUsByNumbers(request.Items.Select(i => i.SKUNumber).ToArray());

            var changedItems = request.Items.Select(i =>
                {
                    var sku = kenticoSkus.Single(s => s.SKUNumber == i.SKUNumber);
                    return CreateChangedItem(i, sku);
                }
            ).ToList();

            var requestDto = new OrderManualUpdateRequestDto
            {
                OrderId = request.OrderId,
                Items = changedItems,
                TotalPrice = totalPrice,
                TotalTax = totalTax,
                TotalShipping = totalShipping
            };

            // TODO call update order
            /*updateService.UpdateOrder()*/

            //TODO for inventory products, update Available items with diff
        }

        ItemUpdateDto CreateChangedItem(OrderItemUpdate item, Sku sku)
        {
            // TODO :
            // check minmax
            // inventory count, mabe involve SellIfNotAvailable
            // pricing model and update price respectively

            orderChecker.CheckMinMaxQuantity(sku, item.Quantity);

            //orderChecker.EnsureInventoryAmount(sku, TODO added and resulted quantity)

            var price = products.GetPriceByCustomModel(66666666/*todo*/, item.Quantity);


            return new ItemUpdateDto
            {
                LineNumber = item.LineNumber,
                UnitCount = item.Quantity
            };
            
        }
    }
}
