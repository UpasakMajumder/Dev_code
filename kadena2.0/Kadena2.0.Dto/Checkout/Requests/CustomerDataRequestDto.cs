using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.Checkout.Requests
{
    public class CustomerDataRequestDto
    {
        [Required]
        public int CustomerId { get; set; }
    }
}