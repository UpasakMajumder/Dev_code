using System.Collections.Generic;

namespace Kadena.WebAPI.Models
{
    public class CheckoutPage
    {
        public CartItems Products { get;set;}
        public DeliveryAddresses DeliveryAddresses { get; set; }
        public DeliveryCarriers DeliveryMethods { get; set; }
        public PaymentMethods PaymentMethods { get; set; }
        public Totals Totals { get; set; }
        public string SubmitLabel { get; set; }
        public string ValidationMessage { get; set; }
    }
}