namespace Kadena.WebAPI.Models
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
}