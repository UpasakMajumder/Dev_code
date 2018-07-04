namespace Kadena.Models.ShoppingCarts
{
    public class ShoppingCartItem
    {
        public int SkuId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
