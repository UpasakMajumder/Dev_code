using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kadena.WebAPI.Models
{
    public class CheckoutPage
    {
        public DeliveryAddresses DeliveryAddresses { get; set; }
        public DeliveryMethods DeliveryMethod { get; set; }
        //public PaymentMethods PaymentMethod { get; set; }
        //public Totals Totals { get; set; }
        public string SubmitLabel { get; set; }
    }
}