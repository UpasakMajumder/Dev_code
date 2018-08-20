namespace Kadena.Dto.Site.Responses
{
    public class DeliveryOptionDto
    {
        public string Provider { get; set; }
        public int ShippingOptionId { get; set; }
        public string CarrierCode { get; set; }
        public string ShippingService { get; set; }
        public string ShippingServiceDisplayName { get; set; }
    }
}
