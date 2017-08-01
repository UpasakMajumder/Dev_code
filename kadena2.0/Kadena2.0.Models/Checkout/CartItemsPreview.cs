using System.Collections.Generic;

namespace Kadena.Models.Checkout
{
    public class CartItemsPreview
    {
        public string EmptyCartMessage { get; set; }

        public CartButton Cart { get; set; }

        public CartPrice TotalPrice { get; set; }

        public List<CartItem> Items { get; set; }
    }
}
