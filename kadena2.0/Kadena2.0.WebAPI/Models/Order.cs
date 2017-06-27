using Kadena.WebAPI.Models.RecentOrders;
using System;
using System.Collections.Generic;

namespace Kadena.WebAPI.Models
{
    public class Order
    {
        public string Id { get; set; }

        public DateTime CreateDate { get; set; }

        public string Status { get; set; }

        public DateTime DeliveryDate { get; set; }

        public IEnumerable<CartItem> Items { get; set; }

        public Button ViewBtn { get; set; }
    }
}