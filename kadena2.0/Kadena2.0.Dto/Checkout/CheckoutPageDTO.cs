namespace Kadena.Dto.Checkout
{
    public class CheckoutPageDTO
    {
        public CartItemsDTO Products { get; set; }
        public DeliveryAddressesDTO DeliveryAddresses { get; set; }
        public PaymentMethodsDTO PaymentMethods { get; set; }
        public SubmitButtonDTO Submit { get; set; }
        public string ValidationMessage { get; set; }
    }
}
