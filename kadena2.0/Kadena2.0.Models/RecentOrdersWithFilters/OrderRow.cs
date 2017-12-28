using System.Collections.Generic;

namespace Kadena.Models.RecentOrders
{
    public class OrderRow
    {
        public OrderDialog dailog { get; set; }
        public List<OrderTableCell> items { get; set; }
    }
}
