using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.Logon.Requests
{
    public class LogonUserRequestDTO
    {
        [Required]
        public string LoginEmail { get; set; }
        public string Password { get; set; }
        public bool KeepLoggedIn { get; set; }
    }
}
