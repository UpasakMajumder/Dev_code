using Kadena.Models;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ITaxEstimationService
    {
        Task<decimal> EstimatePricedItemsTax(DeliveryAddress deliveryAddress, double pricedItemsPrice);
        Task<decimal> EstimateTotalTax(DeliveryAddress deliveryAddress);
    }
}
