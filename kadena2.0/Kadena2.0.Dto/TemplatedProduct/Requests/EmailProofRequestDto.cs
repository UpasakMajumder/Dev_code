using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.TemplatedProduct.Requests
{
    public class EmailProofRequestDto
    {
        [Required]
        public string EmailProofUrl { get; set; }
        public string Message { get; set; }
        [Required]
        public string RecepientEmail { get; set; }
        [Required]
        public string Subject { get; set; }
    }
}
