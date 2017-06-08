using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kadena.WebAPI.Models.SubmitOrder
{
    public class SubmitOrderRequest
    {
        public int DeliveryAddress { get; set; }
        public int DeliveryMethod { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}