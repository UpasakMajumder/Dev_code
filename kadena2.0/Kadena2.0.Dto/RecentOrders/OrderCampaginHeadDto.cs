using System.Collections.Generic;

namespace Kadena.Dto.RecentOrders
{
    public class OrderCampaginHeadDto
    {
        public string placeholder { get; set; }
        public List<OrderCampaginItemDto> items { get; set; }
    }
}