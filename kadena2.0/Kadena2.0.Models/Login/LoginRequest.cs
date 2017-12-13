using Kadena.Helpers;

namespace Kadena.Models.Login
{
    public class LoginRequest
    {
        public string LoginEmail { get; set; }
        public string Password { get; set; }
        public bool KeepLoggedIn { get; set; }

        public ValidationFieldResult Validate()
        {
            if (string.IsNullOrWhiteSpace(LoginEmail))
            {
                return new ValidationFieldResult { Name = "loginEmail", Error = "Kadena.Logon.LoginEmailEmpty" };
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                return new ValidationFieldResult { Name = "password", Error = "Kadena.Logon.LoginEmailEmpty" };
            }
            if (!MailValidator.IsValid(LoginEmail))
            {
                return new ValidationFieldResult { Name = "loginEmail", Error = "Kadena.Logon.InvalidEmail" };
            }

            return null;
        }
    }
}
