using System.Collections.Generic;

namespace Kadena.WebAPI.Models.RecentOrders
{
    public class OrderBody
    {
        public IEnumerable<Order> Rows { get; set; }
    }
}