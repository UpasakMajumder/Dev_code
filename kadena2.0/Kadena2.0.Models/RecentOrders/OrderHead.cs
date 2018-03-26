using Kadena.Models.Common;
using System.Collections.Generic;

namespace Kadena.Models.RecentOrders
{
    public class OrderHead
    {
        public List<string> Headings { get; set; }
        public Pagination PageInfo { get; set; }
        public string NoOrdersMessage { get; set; }
        public IEnumerable<Order> Rows { get; set; }
    }
}