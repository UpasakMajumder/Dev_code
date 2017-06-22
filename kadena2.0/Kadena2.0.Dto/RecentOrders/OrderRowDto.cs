using Kadena.Dto.Order;
using System;
using System.Collections.Generic;

namespace Kadena.Dto.RecentOrders
{
    public class OrderRowDto
    {
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public string OrderStatus { get; set; }
        public DateTime DeliveryDate { get; set; }
        public ButtonDto ViewBtn { get; set; }
    }
}
