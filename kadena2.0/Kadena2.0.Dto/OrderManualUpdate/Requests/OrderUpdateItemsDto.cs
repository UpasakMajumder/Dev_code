using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.OrderManualUpdate.Requests
{
    public class OrderUpdateItemsDto
    {
        [Required]
        public string OrderId { get; set; }
        [Required]
        public IEnumerable<OrderItemUpdateDto> Items { get; set; }
    }
}
