namespace Kadena.Models.Orders
{
    public class ReportLineItem
    {
        public string Name { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string TrackingNumber { get; set; }
    }
}
