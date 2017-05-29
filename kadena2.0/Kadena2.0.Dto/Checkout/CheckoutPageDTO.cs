namespace Kadena.Dto.Checkout
{
    public class CheckoutPageDTO
    {
        public DeliveryAddressesDTO DeliveryAddresses { get; set; }
        public DeliveryMethodsDTO DeliveryMethods { get; set; }
        public PaymentMethodsDTO PaymentMethods { get; set; }
        public TotalsContainerDTO Totals { get; set; }
        public string SubmitLabel { get; set; }
    }
}
