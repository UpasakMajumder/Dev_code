using System;

namespace Kadena.Models.Product
{
    public class Product
    {
        public int Id { get; set; }
        public int NodeId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Availability { get; set; }
        public int  StockItems { get; set; }
        public string DocumentUrl { get; set; }
        public string ImageUrl { get; set; }
        public string ProductType { get; set; }
        public Guid ProductMasterTemplateID { get; set; }
        public Guid ProductChiliWorkgroupID { get; set; }
        public Guid TemplateLowResSettingId { get; set; }
        public Guid TemplateHiResSettingId { get; set; }
        public double Weight { get; set; }
        public bool HiResPdfDownloadEnabled { get; set; }
        public int SkuId { get; set; }
        public string SkuNumber { get; set; }
        public string ProductionTime { get; set; }
        public string ShipTime { get; set; }
        public string ShippingCost { get; set; }
        public bool Use3d { get; set; }
        public string PricingModel { get; set; }
        public string TieredPricingJson { get; set; }
        public string DynamicPricingJson { get; set; }
        public bool SendPriceToERP { get; set; }
        public string UnitOfMeasure { get; set; }

        public bool HasProductTypeFlag(string productType)
        {
            return ProductTypes.IsOfType(ProductType, productType);
        }

        public bool IsTemplateLowResSettingMissing => HasProductTypeFlag(ProductTypes.TemplatedProduct) && TemplateLowResSettingId == Guid.Empty;
    }
}