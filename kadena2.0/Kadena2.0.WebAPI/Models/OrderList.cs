using System.Collections.Generic;

namespace Kadena.WebAPI.Models
{
    public class OrderList
    {
        public int TotalCount { get; set; }

        public IEnumerable<Order> Orders { get; set; }
    }
}