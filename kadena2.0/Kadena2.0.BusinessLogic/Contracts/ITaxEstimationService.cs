using Kadena.Models;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ITaxEstimationService
    {
        Task<decimal> EstimateTax(DeliveryAddress deliveryAddress, decimal pricedItemsPrice, decimal shippingCost);
    }
}
