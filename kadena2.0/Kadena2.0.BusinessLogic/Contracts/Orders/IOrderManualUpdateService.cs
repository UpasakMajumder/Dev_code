using Kadena.Models.Orders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts.Orders
{
    public interface IOrderManualUpdateService
    {
        Task UpdateOrderItems(IEnumerable<OrderItemUpdate> items);
    }
}
