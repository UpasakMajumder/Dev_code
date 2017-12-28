using System.Collections.Generic;

namespace Kadena.Models.RecentOrders
{
    public class RecentOrderList
    {
        public int TotalCount { get; set; }

        public IEnumerable<OrderRow> Orders { get; set; }
    }
}