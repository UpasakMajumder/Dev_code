namespace Kadena.Dto.ViewOrder.Responses
{
    public class OrderedItemDTO
    {
        public int id { get; set; }
        public string image { get; set; }
        public string template { get; set; }
        public string mailingList { get; set; }
        public string shippingDate { get; set; }
        public string trackingId { get; set; }
        public string price { get; set; }
        public string quantityPrefix { get; set; }
        public int quantity { get; set; }
        public string downloadPdfURL { get; set; }
    }
}