namespace Kadena.Dto.ViewOrder.MicroserviceResponses.History
{
    public class OrderHistoryUpdateDto
    {
        public string OrderId { get; set; }
        public long ServiceRecordDateTimeCreated { get; set; }
        public int AdapterDateTimeReceived { get; set; }
        public string ClientId { get; set; }
        public string UserId { get; set; }
        public string ErpClientId { get; set; }
        public string SiteName { get; set; }
        public int RecordTypeId { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string Description { get; set; }
        public string SapOrderId { get; set; }
        public int IsDeleted { get; set; }
        public int DateTimeDeleted { get; set; }
        public string UID { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalTax { get; set; }
        public decimal TotalShipping { get; set; }
        public ItemDto[] Items { get; set; }
        public PaymentoptionsDto PaymentOptions { get; set; }
    }
}
