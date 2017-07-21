namespace Kadena.Models.OrderDetail
{
    public class ShippingInfo
    {
        public string Title { get; set; }
        public string DeliveryMethod { get; set; }
        public string Address { get; set; }
        public Tracking Tracking { get; set; }
    }
}