using System.Collections.Generic;

namespace Kadena.Models.OrderDetail
{
    public class OrderedItemsGroup
    {
        public OrderedItemsGroupTracking Tracking { get; set; }
        public OrderedItemsGroupShippingDate ShippingDate { get; set; }
        public IList<OrderedItem> Orders { get; set; }
    }
}
