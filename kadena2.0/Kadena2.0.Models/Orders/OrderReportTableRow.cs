using Kadena.Models.Common;

namespace Kadena.Models.Orders
{
    public class OrderReportTableRow
    {
        public TableCell lineNumber { get; set; }
        public TableCell site { get; set; }
        public TableCell orderNumber { get; set; }
        public TableCell createDate { get; set; }
        public TableCell user { get; set; }
        public TableCell name { get; set; }
        public TableCell sku { get; set; }
        public TableCell quantity { get; set; }
        public TableCell price { get; set; }
        public TableCell status { get; set; }
        public TableCell shippingDate { get; set; }
        public TableCell trackingNumber { get; set; }
        public TableCell shippedQuantity { get; set; }
        public TableCell shippingMethod { get; set; }
        public TableCell trackingInfoId { get; set; }
    }
}
