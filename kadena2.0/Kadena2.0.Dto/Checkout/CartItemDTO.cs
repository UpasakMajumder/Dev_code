namespace Kadena.Dto.Checkout
{
    public class CartItemDTO
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Template { get; set; }
        public bool IsMailingList { get; set; }
        public string MailingList { get; set; }
        public string Delivery { get; set; }
        public string PricePrefix { get; set; }
        public string Price { get; set; }
        public bool IsEditable { get; set; }
        public string QuantityPrefix { get; set; }
        public int Quantity { get; set; }
        public int StockQuantity { get; set; }
        public string EditorURL { get; set; }
    }
}
