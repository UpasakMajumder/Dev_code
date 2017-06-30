using System.Collections.Generic;

namespace Kadena.WebAPI.Models.Checkout
{
    public class CartItems
    {
        public string Number { get; set; }
        public List<CartItem> Items { get; set; }

        public void HidePrices()
        {
            Items.ForEach(item =>
           {
               item.PriceText = string.Empty;
               item.PricePrefix = string.Empty;
           });
        }
    }
}