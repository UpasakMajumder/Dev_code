namespace Kadena.Models.Login
{
    public class LoginRequest
    {
        public string LoginEmail { get; set; }
        public string Password{ get; set; }
        public bool KeepLoggedIn { get; set; }

        public void Validate()
        {
            #region Validation
            /*
            if (string.IsNullOrWhiteSpace(loginEmail))
            {
                return new LogonUserResultDTO { success = false, errorPropertyName = "loginEmail", errorMessage = ResHelper.GetString("Kadena.Logon.LoginEmailEmpty", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                return new LogonUserResultDTO { success = false, errorPropertyName = "password", errorMessage = ResHelper.GetString("Kadena.Logon.PasswordEmpty", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (!IsEmailValid(loginEmail))
            {
                return new LogonUserResultDTO { success = false, errorPropertyName = "loginEmail", errorMessage = ResHelper.GetString("Kadena.Logon.InvalidEmail", LocalizationContext.CurrentCulture.CultureCode) };
            }
            UserInfo user = UserInfoProvider.GetUserInfo(loginEmail);
            if (user == null)
            {
                return new LogonUserResultDTO { success = false, errorPropertyName = "loginEmail", errorMessage = ResHelper.GetString("Kadena.Logon.LogonFailed", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (!user.IsInSite(SiteContext.CurrentSiteName))
            {
                return new LogonUserResultDTO { success = false, errorPropertyName = "loginEmail", errorMessage = ResHelper.GetString("Kadena.Logon.LogonFailed", LocalizationContext.CurrentCulture.CultureCode) };
            }
            */
            #endregion
        }
    }
}
