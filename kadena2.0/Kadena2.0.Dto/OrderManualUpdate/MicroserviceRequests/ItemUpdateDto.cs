namespace Kadena.Dto.OrderManualUpdate.MicroserviceRequests
{
    public class ItemUpdateDto
    {
        public int LineNumber { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
