namespace Kadena.Models.OrderDetail
{
    public class OrderedItems
    {
        public OrderedItemsSection ShippedItems { get; set; }
        public OrderedItemsSection OpenItems { get; set; }
        public OrderedItemsSection MailingItems { get; set; }

        public void HidePrices()
        {
            if (ShippedItems != null)
                ShippedItems.HidePrices();

            if (OpenItems != null)
                OpenItems.HidePrices();

            if (MailingItems != null)
                MailingItems.HidePrices();
        }

        public void OrderItemsByLineNumber()
        {
            if (ShippedItems != null)
                ShippedItems.OrderItemsByLineNumber();

            if (OpenItems != null)
                OpenItems.OrderItemsByLineNumber();

            if (MailingItems != null)
                MailingItems.OrderItemsByLineNumber();
        }
    }
}
