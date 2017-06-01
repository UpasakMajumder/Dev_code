namespace Kadena.WebAPI.Models
{
    public class CheckoutPage
    {
        public DeliveryAddresses DeliveryAddresses { get; set; }
        public DeliveryMethods DeliveryMethods { get; set; }
        public PaymentMethods PaymentMethods { get; set; }
        public Totals Totals { get; set; }
        public string SubmitLabel { get; set; }
        public string ValidationMessage { get; set; }
    }
}