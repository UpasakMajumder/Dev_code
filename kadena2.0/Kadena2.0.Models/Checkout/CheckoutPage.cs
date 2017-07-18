namespace Kadena.Models.Checkout
{
    public class CheckoutPage
    {
        public CartItems Products { get;set;}
        public DeliveryAddresses DeliveryAddresses { get; set; }
        public DeliveryCarriers DeliveryMethods { get; set; }
        public PaymentMethods PaymentMethods { get; set; }
        public Totals Totals { get; set; }
        public SubmitButton Submit { get; set; }
        public string ValidationMessage { get; set; }

        public void SetDisplayType()
        {
            if (Products.Items.TrueForAll(p => p.IsMailingList))
            {
                DeliveryAddresses.IsDeliverable = false;
            }
        }
    }
}