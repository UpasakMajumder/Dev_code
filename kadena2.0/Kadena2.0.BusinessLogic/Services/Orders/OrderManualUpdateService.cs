using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Models.Orders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class OrderManualUpdateService : IOrderManualUpdateService
    {

        public async Task UpdateOrderItems(IEnumerable<OrderItemUpdate> items)
        {

        }
    }
}
