namespace Kadena.Dto.ViewOrder.Responses
{
    public class ShippingInfoDTO
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string DeliveryMethod { get; set; }
        public string Address { get; set; }
        public TrackingDTO Tracking { get; set; }
    }
}