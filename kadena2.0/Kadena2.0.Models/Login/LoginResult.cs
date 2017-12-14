using Kadena.Helpers;

namespace Kadena.Models.Login
{
    public class LoginResult
    {
        public bool LogonSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorPropertyName { get; set; }        
    }
}
