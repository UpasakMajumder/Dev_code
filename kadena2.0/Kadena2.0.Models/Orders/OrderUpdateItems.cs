using System.Collections.Generic;

namespace Kadena.Models.Orders
{
    public class OrderUpdateItems: OrderUpdate
    {
        public IEnumerable<OrderItemUpdate> Items { get; set; }
    }
}
