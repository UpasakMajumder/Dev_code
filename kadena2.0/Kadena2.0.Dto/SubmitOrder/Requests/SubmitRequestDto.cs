namespace Kadena.Dto.SubmitOrder.Requests
{
    public class SubmitRequestDto
    {
        public int DeliveryAddress { get; set; }
        public int DeliveryMethod { get; set; }
        public PaymentMethodDto PaymentMethod { get; set; }
    }
}