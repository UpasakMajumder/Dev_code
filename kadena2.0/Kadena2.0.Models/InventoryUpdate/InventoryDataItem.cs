namespace Kadena.Models.InventoryUpdate
{
    class InventoryDataItem
    {
        public string ClientId { get; set; }
        public string Id { get; set; }
        public string ErpName { get; set; }
        public decimal AvailableQty { get; set; }
        public decimal TotalQty { get; set; }
    }
}
