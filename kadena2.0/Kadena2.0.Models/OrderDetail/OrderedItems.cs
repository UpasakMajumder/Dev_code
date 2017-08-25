using System.Collections.Generic;

namespace Kadena.Models.OrderDetail
{
    public class OrderedItems
    {
        public string Title { get; set; }
        public IList<OrderedItem> Items { get; set; }

        public void HidePrices()
        {
            if (Items != null)
            {
                foreach (var i in Items)
                {
                    i.Price = string.Empty;
                }
            }
        }
    }
}