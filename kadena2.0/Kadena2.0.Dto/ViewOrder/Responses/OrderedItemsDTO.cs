using System.Collections.Generic;

namespace Kadena.Dto.ViewOrder.Responses
{
    public class OrderedItemsDTO
    {
        public string Title { get; set; }
        public IList<OrderedItemDTO> Items { get; set; }
    }
}