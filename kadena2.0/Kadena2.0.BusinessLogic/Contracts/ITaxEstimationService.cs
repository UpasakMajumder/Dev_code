using Kadena.Models;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ITaxEstimationService
    {
        Task<decimal> EstimateTotalTax(DeliveryAddress deliveryAddress);
    }
}
