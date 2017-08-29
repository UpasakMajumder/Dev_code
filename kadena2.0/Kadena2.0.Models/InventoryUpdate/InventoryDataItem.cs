namespace Kadena.Models.InventoryUpdate
{
    class InventoryDataItem
    {
        public string ClientId { get; set; }
        public string Id { get; set; }
        public string ErpName { get; set; }
        public int AvailableQty { get; set; }
        public int TotalQty { get; set; }
    }
}
