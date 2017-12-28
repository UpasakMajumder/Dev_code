using System.Collections.Generic;

namespace Kadena.Models.RecentOrders
{
    public class OrderCampaginHead
    {
        public string placeholder { get; set; }
        public List<OrderCampaginItem> items { get; set; }
    }
}