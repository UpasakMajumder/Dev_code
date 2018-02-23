using System.Collections.Generic;

namespace Kadena.Dto.Checkout.Responses
{
    public class CartItemsPreviewDTO
    {
        public string EmptyCartMessage { get; set; }

        public CartPriceDTO SummaryPrice { get; set; }

        public List<CartItemPreviewDTO> Items { get; set; }
    }
}
