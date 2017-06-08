namespace Kadena.WebAPI.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Template { get; set; }
        public bool IsMailingList { get; set; }
        public string MailingList { get; set; }
        public string Delivery { get; set; }
        public string PricePrefix { get; set; }
        public double Price { get; set; }
        public bool IsEditable { get; set; }
        public string QuantityPrefix { get; set; }
        public int Quantity { get; set; }
    }
}