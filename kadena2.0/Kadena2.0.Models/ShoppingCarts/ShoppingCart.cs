using System.Collections.Generic;

namespace Kadena.Models.ShoppingCarts
{
    public class ShoppingCart
    {
        public List<ShoppingCartItem> Items { get; set; }

        public decimal TotalTax { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal PricedItemsTax { get; set; }
    }
}
