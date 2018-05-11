using Kadena.Models.Checkout;
using Kadena.Models.Common;
using System;
using System.Collections.Generic;

namespace Kadena.Models
{
    public class Order
    {
        public string Id { get; set; }

        public DateTime CreateDate { get; set; }

        public string Status { get; set; }

        public DateTime? ShippingDate { get; set; }

        public IEnumerable<CheckoutCartItem> Items { get; set; }

        public Button ViewBtn { get; set; }

        public Campaign campaign { get; set; }
    }
}