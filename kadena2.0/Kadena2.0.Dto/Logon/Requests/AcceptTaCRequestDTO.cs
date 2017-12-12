using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.Logon.Requests
{
    public class AcceptTaCRequestDTO
    {
        [Required]
        public string LoginEmail { get; set; }
        public string Password { get; set; }
    }
}
