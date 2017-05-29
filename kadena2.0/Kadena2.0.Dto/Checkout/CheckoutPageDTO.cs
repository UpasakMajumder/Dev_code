namespace Kadena.Dto.Checkout
{
    public class CheckoutPageDTO
    {
        public DeliveryAddressesDTO DeliveryAddresses { get; set; }
        public DeliveryMethodsDTO DeliveryMethod { get; set; }
        public PaymentMethodsDTO PaymentMethod { get; set; }
        public TotalsContainerDTO Totals { get; set; }
        public string SubmitLabel { get; set; }
    }
}
