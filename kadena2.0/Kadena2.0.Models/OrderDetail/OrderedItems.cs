using System.Collections.Generic;

namespace Kadena.Models.OrderDetail
{
    public class OrderedItems
    {
        public string Title { get; set; }
        public IList<OrderedItem> Items { get; set; }
    }
}