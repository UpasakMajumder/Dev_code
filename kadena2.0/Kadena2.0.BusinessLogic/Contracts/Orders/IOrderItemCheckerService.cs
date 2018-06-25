using Kadena.Models.Product;

namespace Kadena.BusinessLogic.Contracts.Orders
{
    public interface IOrderItemCheckerService
    {
        void EnsureInventoryAmount(Sku sku, int addedQuantity, int resultedQuantity);
        void CheckMinMaxQuantity(Sku sku, int totalAmountAfterAdding);
    }
}
