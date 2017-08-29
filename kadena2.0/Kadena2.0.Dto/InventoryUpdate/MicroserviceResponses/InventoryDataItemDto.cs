namespace Kadena.Dto.InventoryUpdate.MicroserviceResponses
{
    public class InventoryDataItemDto
    {
        public string ClientId { get; set; }
        public string Id { get; set; }
        public string ErpName { get; set; }
        public int AvailableQty { get; set; }
        public int TotalQty { get; set; }
    }
}
