using System.Collections.Generic;

namespace Kadena.Dto.ViewOrder.Responses
{
    public class OrderedItemsSectionDTO
    {
        public string Title { get; set; }
        public IList<OrderedItemsGroupDTO> Items { get; set; }
    }
}
