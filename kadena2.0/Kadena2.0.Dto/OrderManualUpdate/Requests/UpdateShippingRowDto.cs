namespace Kadena.Dto.OrderManualUpdate.Requests
{
    public class UpdateShippingRowDto
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
