using Kadena.Models.Common;
using System.Collections.Generic;

namespace Kadena.Models.RecentOrders
{
    public class OrderHeadBlock
    {
        public List<string> headers { get; set; }
        public Pagination PageInfo { get; set; }
        public string NoOrdersMessage { get; set; }
        public IEnumerable<OrderRow> rows { get; set; }
    }
}