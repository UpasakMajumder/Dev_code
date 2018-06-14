using System.Collections.Generic;

namespace Kadena.Models.Orders
{
    public class OrderUpdate
    {
        public string OrderId { get; set; }
        public int CustomerId { get; set; }
        public IEnumerable<OrderItemUpdate> Items { get; set; }
    }
}
