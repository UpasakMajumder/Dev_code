using System.Collections.Generic;

namespace Kadena.Models.RecentOrders
{
    public class OrderBody
    {
        public IEnumerable<Order> Rows { get; set; }
    }
}