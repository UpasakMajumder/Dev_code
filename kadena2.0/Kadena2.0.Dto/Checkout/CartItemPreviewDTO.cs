namespace Kadena.Dto.Checkout
{
    public class CartItemPreviewDTO
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string TemplatePrefix { get; set; }
        public string Template { get; set; }
        public bool IsMailingList { get; set; }
        public string MailingList { get; set; }
        public string PricePrefix { get; set; }
        public string Price { get; set; }
        public string UnitOfMeasure { get; set; }
        public string QuantityPrefix { get; set; }
        public int Quantity { get; set; }
    }
}