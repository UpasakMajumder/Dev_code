namespace Kadena.Dto.ViewOrder.Responses
{
    public class OrderedItemsDTO
    {
        public OrderedItemsSectionDTO ShippedItems { get; set; }
        public OrderedItemsSectionDTO OpenItems { get; set; }
        public OrderedItemsSectionDTO MailingItems { get; set; }
    }
}