namespace Kadena.Dto.Checkout
{
    public class CheckoutPageDTO
    {
        public DeliveryAddressesContainerDTO DeliveryAddresses { get; set; }
        public DeliveryMethodContainerDTO DeliveryMethod { get; set; }
        public PaymentMethodContainerDTO PaymentMethod { get; set; }
        public TotalsContainerDTO Totals { get; set; }
        public string SubmitLabel { get; set; }
    }
}
