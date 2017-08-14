namespace Kadena.Models
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
    }

    public class ProductTypes
    {
        public static string POD => "KDA.POD";
        public static string StaticProduct => "KDA.StaticProduct";
        public static string InventoryProduct => "KDA.InventoryProduct";
        public static string ProductWithAddOns => "KDA.ProductWithAddOns";
        public static string MailingProduct => "KDA.MailingProduct";
        public static string TemplatedProduct => "KDA.TemplatedProduct";
    }
}