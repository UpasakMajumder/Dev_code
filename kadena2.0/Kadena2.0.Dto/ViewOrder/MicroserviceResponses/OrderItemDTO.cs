namespace Kadena.Dto.ViewOrder.MicroserviceResponses
{
    public class OrderItemDTO
    {
        public string Type { get; set; }
        public int Qty { get; set; }
        public string TrackingId { get; set; }
        public string Name { get; set; }
        public string MailingList { get; set; }
        public string FileUrl { get; set; }
        public double TotalPrice { get; set; }
    }
}
