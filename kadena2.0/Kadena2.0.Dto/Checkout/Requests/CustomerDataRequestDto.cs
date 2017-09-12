using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.Checkout.Requests
{
    public class CustomerDataRequestDto
    {
        [Required]
        public int SiteId { get; set; }

        [Required]
        public int CustomerId { get; set; }
    }
}