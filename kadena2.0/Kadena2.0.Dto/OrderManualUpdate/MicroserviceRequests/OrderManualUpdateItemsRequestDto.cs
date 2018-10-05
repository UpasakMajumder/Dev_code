using System.Collections.Generic;

namespace Kadena.Dto.OrderManualUpdate.MicroserviceRequests
{
    public class OrderManualUpdateItemsRequestDto: OrderManualUpdateRequestDto
    {
        public List<ItemUpdateDto> Items { get; set; }

        public decimal TotalTax { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TotalShipping { get; set; }

        public decimal PricedItemsTax { get; set; }
    }
}
