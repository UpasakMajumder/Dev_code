namespace Kadena.Models.Shipping
{
    public class TrackingInfo
    {
        public string ItemId { get; set; }
        public string Id { get; set; }
        public string Url { get; set; }
        public int QuantityShipped { get; set; }
        public string ShippingDate { get; set; }
        public TrackingInfoShippingMethod ShippingMethod { get; set; }

        public override string ToString()
        {
            return Id;
        }
    }
}