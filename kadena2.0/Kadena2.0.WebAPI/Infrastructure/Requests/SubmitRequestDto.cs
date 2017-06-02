using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kadena.WebAPI.Infrastructure.Requests
{
    public class SubmitRequestDto
    {
        public int DeliveryAddress { get; set; }
        public int DeliveryMethod { get; set; }
        public PaymentMethodDto PaymentMethod { get; set; }
    }
}