namespace Kadena.Dto.Payment.CreditCard.MicroserviceRequests
{
    public class CreateCustomerContainerRequestDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Phone { get; set; }
        public AddressDto ShippingAddress { get; set; }
        public AddressDto BillingAddress { get; set; }
        public string AdditionalData { get; set; }
    }
}
