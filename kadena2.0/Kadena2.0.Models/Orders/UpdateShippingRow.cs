namespace Kadena.Models.Orders
{
    public class UpdateShippingRow
    {
        public int LineNumber { get; set; }
        public string OrderNumber { get; set; }
        public int QuantityShipped { get; set; }
        public string ShippingDate { get; set; }
        public string ShippingService { get; set; }
    }
}
