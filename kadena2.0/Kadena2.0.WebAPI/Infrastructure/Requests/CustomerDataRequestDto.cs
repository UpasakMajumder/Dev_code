using System.ComponentModel.DataAnnotations;

namespace Kadena.WebAPI.Infrastructure.Requests
{
    public class CustomerDataRequestDto
    {
        [Required]
        public int CustomerId { get; set; }
    }
}