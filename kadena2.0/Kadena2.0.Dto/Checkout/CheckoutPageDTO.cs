namespace Kadena.Dto.Checkout
{
    public class CheckoutPageDTO
    {
        public DeliveryAddressesDTO DeliveryAddresses { get; set; }
        public DeliveryMethodsDTO DeliveryMethod { get; set; }
        public PaymentMethodsDTO PaymentMethods { get; set; }
        public TotalsDTO Totals { get; set; }
        public string SubmitLabel { get; set; }
        public string ValidationMessage { get; set; }
    }
}
