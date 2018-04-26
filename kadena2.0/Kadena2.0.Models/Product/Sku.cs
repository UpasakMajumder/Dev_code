namespace Kadena.Models.Product
{
    public class Sku
    {
        public int SkuId { get; set; }
        public double Weight { get; set; }
        public bool NeedsShipping { get; set; }
        public int AvailableItems { get; set; }
        public bool SellOnlyIfAvailable { get; set; }
        public bool HiResPdfDownloadEnabled { get; set; }
        public bool ApprovalRequired { get; set; }
    }
}