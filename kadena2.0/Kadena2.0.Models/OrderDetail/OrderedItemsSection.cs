using System.Collections.Generic;
using System.Linq;

namespace Kadena.Models.OrderDetail
{
    public class OrderedItemsSection
    {
        public string Title { get; set; }
        public IList<OrderedItemsGroup> Items { get; set; }

        public void HidePrices()
        {
            if (Items == null)
                return;

            foreach (var i in Items)
            {
                foreach (var o in i.Orders)
                {
                    o.Price = string.Empty;
                }
            }
        }

        public void OrderItemsByLineNumber()
        {
            if (Items == null)
                return;

            foreach(var item in Items)
            {
                item.Orders.OrderBy(x => x.LineNumber);
            }
        }
    }
}