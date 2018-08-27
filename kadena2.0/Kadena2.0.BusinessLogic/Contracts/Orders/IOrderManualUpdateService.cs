using Kadena.Models.Orders;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts.Orders
{
    public interface IOrderManualUpdateService
    {
        Task<OrderUpdateResult> UpdateOrder(OrderUpdate request);
        Task<(bool, string)> UpdateOrdersShippings(UpdateShippingRow[] request);
    }
}
