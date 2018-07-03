using Kadena.Models;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ITaxEstimationService
    {
        Task<decimal> EstimateTax(DeliveryAddress deliveryAddress, double pricedItemsPrice);
    }
}
