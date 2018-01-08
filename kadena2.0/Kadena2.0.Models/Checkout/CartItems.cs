using System.Collections.Generic;

namespace Kadena.Models.Checkout
{
    public class CartItems
    {
        public string Number { get; set; }
        public string ProductionTimeLabel { get; set; }
        public string ShipTimeLabel { get; set; }
        public List<CartItem> Items { get; set; }
        public CartPrice SummaryPrice { get; set; }
        public ButtonLabels ButtonLabels { get; set; }
        public void HidePrices()
        {
            Items.ForEach(item =>
           {
               item.PriceText = string.Empty;
               item.PricePrefix = string.Empty;
           });

            if (SummaryPrice != null)
            {
                SummaryPrice.Price = string.Empty;
                SummaryPrice.PricePrefix = string.Empty;
            }
        }
    }
}