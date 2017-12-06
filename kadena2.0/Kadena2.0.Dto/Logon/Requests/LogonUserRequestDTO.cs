using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.Logon.Requests
{
    public class LogonUserRequestDTO
    {
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
        public bool KeepMeLoggedIn { get; set; }
    }
}
