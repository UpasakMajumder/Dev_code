using System.Collections.Generic;

namespace Kadena.WebAPI.Models.OrderDetail
{
    public class OrderedItems
    {
        public string Title { get; set; }
        public IList<OrderedItem> Items { get; set; }
    }
}