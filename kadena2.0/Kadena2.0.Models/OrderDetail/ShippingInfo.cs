namespace Kadena.Models.OrderDetail
{
    public class ShippingInfo
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string DeliveryMethod { get; set; }
        public DeliveryAddress Address { get; set; }
    }
}