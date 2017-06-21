using System.Collections.Generic;

namespace Kadena.Dto.RecentOrders
{
    public class OrderRowDto
    {
        public string OrderNumber { get; set; }
        public string OrderDate { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public string OrderStatus { get; set; }
        public string DeliveryDate { get; set; }
        public ButtonDto ViewBtn { get; set; }
    }
}
