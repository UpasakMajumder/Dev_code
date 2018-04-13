using System;
using System.Collections.Generic;

namespace Kadena.Models.Orders
{
    public class OrderReport
    {
        public List<ReportLineItem> Items { get; set; } = new List<ReportLineItem>();

        public string Url { get; set; }
        public string Site { get; set; }
        public string Number { get; set; }
        public DateTime OrderingDate { get; set; }
        public DateTime? ShippingDate { get; set; }
        public string Status { get; set; }
        public string User { get; set; }
    }

    public class ReportLineItem
    {
        public string Name { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string TrackingNumber { get; set; }
    }
}
