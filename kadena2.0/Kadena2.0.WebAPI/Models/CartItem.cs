namespace Kadena.WebAPI.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string ProductType { get; set; }
        public string Image { get; set; }
        public string Template { get; set; }
        public string EditorTemplateId { get; set; }
        public int ProductPageId { get; set; }

        public int SKUID { get;set;}

        public bool IsMailingList
        {
            get
            {
                return ProductType.Contains("KDA.MailingProduct");
            }
        }
        public string MailingListName { get; set; }
        public string MailingListGuid { get; set; }
        public string Delivery { get; set; }
        public string PricePrefix { get; set; }
        public double Price { get; set; }
        public bool IsEditable
        {
            get
            {
                return ProductType.Contains("KDA.TemplatedProduct");
            }
        }
        public string QuantityPrefix { get; set; }
        public int Quantity { get; set; }
        public int StockQuantity { get; set; }

        public string EditorURL
        {
            get
            {
                return $"/products/product-tools/product-editor?id={ProductPageId}&skuid={SKUID}&templateid={EditorTemplateId}";
            }
        }
    }
}