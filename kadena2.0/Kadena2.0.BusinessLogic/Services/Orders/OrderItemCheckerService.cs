using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Infrastructure.Exceptions;
using Kadena.Models.Product;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class OrderItemCheckerService : IOrderItemCheckerService
    {
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
