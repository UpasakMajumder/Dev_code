namespace Kadena.Models.SubmitOrder
{
    public class SubmitOrderRequest
    {
        public int DeliveryAddress { get; set; }
        public int DeliveryMethod { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}