using System.Collections.Generic;

namespace Kadena.Dto.Checkout
{
    public class CartItemsPreviewDTO
    {
        public string EmptyCartMessage { get; set; }

        public CartPriceDTO SummaryPrice { get; set; }

        public List<CartItemPreviewDTO> Items { get; set; }
    }
}
