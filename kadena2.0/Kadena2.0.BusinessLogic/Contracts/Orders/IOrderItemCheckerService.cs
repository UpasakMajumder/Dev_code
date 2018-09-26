using Kadena.Models.Product;

namespace Kadena.BusinessLogic.Contracts.Orders
{
    public interface IOrderItemCheckerService
    {
        void CheckMinMaxQuantity(Sku sku, int totalAmountAfterAdding);
    }
}
