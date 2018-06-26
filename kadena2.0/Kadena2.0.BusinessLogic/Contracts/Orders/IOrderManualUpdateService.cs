using Kadena.Models.Orders;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts.Orders
{
    public interface IOrderManualUpdateService
    {
        Task<OrderUpdateResult> UpdateOrder(OrderUpdate request);
    }
}
