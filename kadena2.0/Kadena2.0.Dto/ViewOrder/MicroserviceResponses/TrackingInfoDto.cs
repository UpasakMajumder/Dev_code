namespace Kadena.Dto.ViewOrder.MicroserviceResponses
{
    public class TrackingInfoDto
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public int QuantityShipped { get; set; }
        public string ShippingDate { get; set; }
    }
}