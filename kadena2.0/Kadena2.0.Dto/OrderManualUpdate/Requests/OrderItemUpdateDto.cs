using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.OrderManualUpdate.Requests
{
    public class OrderItemUpdateDto
    {
        [Required]
        public int LineNumber { get; set; }
        [Required]
        public string SKUNumber { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
