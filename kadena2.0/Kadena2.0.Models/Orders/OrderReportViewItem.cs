namespace Kadena.Models.Orders
{
    public class OrderReportViewItem
    {
        public string Url { get; set; }
        public string Site { get; set; }
        public string Number { get; set; }
        public string OrderingDate { get; set; }
        public string User { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public string ShippingDate { get; set; }
        public string TrackingNumber { get; set; }
    }
}
