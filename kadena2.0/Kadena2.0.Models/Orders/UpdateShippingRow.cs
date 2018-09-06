namespace Kadena.Models.Orders
{
    public class UpdateShippingRow
    {
        public int LineNumber { get; set; }
        public string OrderNumber { get; set; }
        public int ShippedQuantity { get; set; }
        public string ShippingDate { get; set; }
        public string ShippingMethod { get; set; }
        public string TrackingInfoId { get; set; }
        public string TrackingNumber { get; set; }
    }
}
