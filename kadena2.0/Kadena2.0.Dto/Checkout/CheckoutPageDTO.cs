namespace Kadena.Dto.Checkout
{
    public class CheckoutPageDTO
    {
        public CartEmptyInfoDTO EmptyCart { get; set; }
        public string Message { get; set; }
        public CartItemsDTO Products { get; set; }
        public DeliveryAddressesDTO DeliveryAddresses { get; set; }
        public PaymentMethodsDTO PaymentMethods { get; set; }
        public SubmitButtonDTO Submit { get; set; }
        public NotificationEmailDto EmailConfirmation { get; set; }
        public string ValidationMessage { get; set; }
    }
}
