using System.Collections.Generic;

namespace Kadena.Models
{
    public class OrderList
    {
        public int TotalCount { get; set; }

        public IEnumerable<Order> Orders { get; set; }
    }
}