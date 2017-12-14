using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.Logon.Requests
{
    public class CheckTaCRequestDTO
    {
        [Required]
        public string LoginEmail { get; set; }
        public string Password { get; set; }
        public bool KeepMeLoggedIn { get; set; }
    }
}
