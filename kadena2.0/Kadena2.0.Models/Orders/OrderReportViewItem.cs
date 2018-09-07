using Kadena.Models.Shipping;
using System.Collections.Generic;

namespace Kadena.Models.Orders
{
    public class OrderReportViewItem
    {
        public int LineNumber { get; set; }
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
        public IEnumerable<TrackingInfo> TrackingInfos { get; set; }
    }
}
