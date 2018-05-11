using System.Collections.Generic;

namespace Kadena.Models.Checkout
{
    public class CartItemsPreview
    {
        public string EmptyCartMessage { get; set; }
        public CartPrice SummaryPrice { get; set; }

        public List<CheckoutCartItem> Items { get; set; }
    }
}
