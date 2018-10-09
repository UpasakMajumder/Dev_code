using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Infrastructure.Exceptions;
using Kadena.Models.Product;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class OrderItemCheckerService : IOrderItemCheckerService
    {
        private readonly IKenticoResourceService resources;

        public OrderItemCheckerService(IKenticoResourceService resources)
        {
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
        }

        public void EnsureInventoryAmount(Sku sku, int addedQuantity, int resultedQuantity)
        {
            if (sku.SellOnlyIfAvailable)
            {
                var availableQuantity = sku.AvailableItems;

                if (addedQuantity > availableQuantity)
                {
                    throw new ArgumentException(resources.GetResourceString("Kadena.Product.LowerNumberOfAvailableProducts"));
                }

                if (resultedQuantity > availableQuantity)
                {
                    var errorText = string.Format(resources.GetResourceString("Kadena.Product.ItemsInCartExceeded"),
                                                  resultedQuantity - addedQuantity,
                                                  availableQuantity - resultedQuantity + addedQuantity);

                    throw new ArgumentException(errorText);
                }
            }
        }

        public void CheckMinMaxQuantity(Sku sku, int totalAmountAfterAdding)
        {
            var min = sku?.MinItemsInOrder ?? 0;
            var max = sku?.MaxItemsInOrder ?? 0;

            if (min > 0 && totalAmountAfterAdding < min && totalAmountAfterAdding != 0)
            {
                throw new NotLoggedException("Cannot order less than minimal count of items");
            }

            if (max > 0 && totalAmountAfterAdding > max)
            {
                throw new NotLoggedException("Cannot order more than maximal count of items");
            }
        }
    }
}
