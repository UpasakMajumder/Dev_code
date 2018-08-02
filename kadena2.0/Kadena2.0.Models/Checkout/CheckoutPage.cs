namespace Kadena.Models.Checkout
{
    public class CheckoutPage
    {
        public CartEmptyInfo EmptyCart { get; set; }
        public CartItems Products { get;set;}
        public DeliveryAddresses DeliveryAddresses { get; set; }
        public PaymentMethods PaymentMethods { get; set; }
        public DeliveryDate DeliveryDate { get; set; }
        public SubmitButton Submit { get; set; }
        public NotificationEmail EmailConfirmation { get; set; }
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