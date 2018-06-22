namespace Kadena.Models.Product
{
    public class Sku
    {
        public int SkuId { get; set; }
        public double Weight { get; set; }
        public bool NeedsShipping { get; set; }
        public int? AvailableItems { get; set; }
        public bool SellOnlyIfAvailable { get; set; }
        public bool HiResPdfDownloadEnabled { get; set; }
        public bool ApprovalRequired { get; set; }
        public int MinItemsInOrder { get; set; }
        public int MaxItemsInOrder { get; set; }
        public string UnitOfMeasure { get; set; }
        public int NumberOfItemsInPackage { get; set; }
        public string SKUNumber { get; set; }
        public bool SendPriceToERP { get; set; }
        public decimal Price { get; set; }
    }
}