namespace Kadena.Dto.ViewOrder.MicroserviceResponses.History
{
    public class ItemDto
    {
        public string ArtWorkKey { get; set; }
        public string ClientId { get; set; }
        public long DateTimeDeleted { get; set; }
        public int IsDeleted { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemNumber { get; set; }
        public string LineNumber { get; set; }
        public string Message { get; set; }
        public string OrderId { get; set; }
        public int Quantity { get; set; }
        public int QuantityShipped { get; set; }
        public int RecordTypeId { get; set; }
        public long ServiceRecordDateTimeCreated { get; set; }
        public string ShipDate { get; set; }
        public int StatusId { get; set; }
        public string TrackingNumber { get; set; }
        public string UID { get; set; }
        public string UserId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
