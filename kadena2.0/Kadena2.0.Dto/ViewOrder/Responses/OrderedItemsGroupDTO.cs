using System.Collections.Generic;

namespace Kadena.Dto.ViewOrder.Responses
{
    public class OrderedItemsGroupDTO
    {
        public OrderedItemsGroupTrackingDTO Tracking { get; set; }
        public OrderedItemsGroupShippingDateDTO ShippingDate { get; set; }
        public IList<OrderedItemDTO> Orders { get; set; }
    }
}
