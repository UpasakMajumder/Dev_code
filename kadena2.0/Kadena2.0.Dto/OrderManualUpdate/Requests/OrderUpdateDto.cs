using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.OrderManualUpdate.Requests
{
    public class OrderUpdateDto
    {
        [Required]
        public string OrderId { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public IEnumerable<OrderItemUpdateDto> Items { get; set; }
    }
}
