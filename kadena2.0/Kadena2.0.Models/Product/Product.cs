using System;

namespace Kadena.Models.Product
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Availability { get; set; }
        public int  StockItems { get; set; }
        public string DocumentUrl { get; set; }
        public string SkuImageUrl { get; set; }
        public string ProductType { get; set; }
        public Guid ProductChiliTemplateID { get; set; }
        public Guid ProductChiliWorkgroupID { get; set; }
        public Guid TemplateLowResSettingId { get; set; }
        public double Weight { get; set; }

        public bool HasProductTypeFlag(string productType)
        {
            return ProductTypes.IsOfType(ProductType, productType);
        }
    }
}