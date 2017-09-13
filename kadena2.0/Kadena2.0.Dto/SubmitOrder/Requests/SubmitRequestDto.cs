using Kadena.Dto.Checkout;

namespace Kadena.Dto.SubmitOrder.Requests
{
    public class SubmitRequestDto
    {
        public DeliveryAddressDTO DeliveryAddress { get; set; }
        public int DeliveryMethod { get; set; }
        public PaymentMethodDto PaymentMethod { get; set; }
    }
}