namespace CMSApp
{
    using CMS.Activities.Loggers;
    using CMS.Helpers;
    using CMS.Localization;
    using CMS.Membership;
    using CMS.SiteProvider;
    using Kadena.Dto.General;
    using Kadena.Dto.Logon;
    using System;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Script.Services;
    using System.Web.Security;
    using System.Web.Services;

    [WebService]
    [ScriptService]
    public class KadenaWebService : System.Web.Services.WebService
    {
        #region Public methods

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public void SignOut()
        {
            AuthenticationHelper.SignOut();
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public LogonUserResultDTO LogonUser(string loginEmail, string password, bool isKeepMeLoggedIn)
        {
            #region Validation

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

            #endregion

            return LogonUserInternal(loginEmail, password, isKeepMeLoggedIn);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public GeneralResultDTO InitialPasswordSetting(string password, string confirmPassword, Guid userGUID)
        {
            #region Validation

            if (string.IsNullOrWhiteSpace(password))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.InitialPasswordSetting.PasswordIsEmpty", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (string.IsNullOrWhiteSpace(confirmPassword))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.InitialPasswordSetting.PasswordIsEmpty", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (password != confirmPassword)
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.InitialPasswordSetting.PasswordsAreNotTheSame", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (!SecurityHelper.CheckPasswordPolicy(password, SiteContext.CurrentSiteName))
            {
                return new GeneralResultDTO { success = false, errorMessage = AuthenticationHelper.GetPolicyViolationMessage(SiteContext.CurrentSiteName) };
            }

            #endregion

            return InitialPasswordSettingInternal(password, confirmPassword, userGUID);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public GeneralResultDTO ContactPersonDetailsChange(Guid userGUID, string firstName, string lastName, string mobile, string phoneNumber)
        {
            return ContactPersonDetailsChangeInternal(userGUID, firstName, lastName, mobile, phoneNumber);
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod]
        public GeneralResultDTO ChangePassword(Guid userGUID, string oldPassword, string newPassword, string confirmPassword)
        {
            #region Validation

            if (string.IsNullOrEmpty(oldPassword))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.Settings.Password.OldPasswordIsEmpty", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.Settings.Password.NewPasswordIsEmpty", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (string.IsNullOrEmpty(confirmPassword))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.Settings.Password.ConfirmPasswordIsEmpty", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (newPassword != confirmPassword)
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.Settings.Password.PasswordsDontMatch", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (!SecurityHelper.CheckPasswordPolicy(newPassword, SiteContext.CurrentSiteName))
            {
                return new GeneralResultDTO { success = false, errorMessage = AuthenticationHelper.GetPolicyViolationMessage(SiteContext.CurrentSiteName) };
            }

            #endregion

            return ChangePasswordInternal(userGUID, oldPassword, newPassword);
        }

        #endregion

        #region Private methods

        private LogonUserResultDTO LogonUserInternal(string loginEmail, string password, bool isKeepMeLoggedIn)
        {
            UserInfo user = null;
            // cookies handling
            CookieHelper.EnsureResponseCookie(FormsAuthentication.FormsCookieName);
            if (isKeepMeLoggedIn)
            {
                CookieHelper.ChangeCookieExpiration(FormsAuthentication.FormsCookieName, DateTime.Now.AddYears(1), false);
            }
            else
            {
                // Extend the expiration of the authentication cookie if required
                if (!AuthenticationHelper.UseSessionCookies && (HttpContext.Current != null) && (HttpContext.Current.Session != null))
                {
                    CookieHelper.ChangeCookieExpiration(FormsAuthentication.FormsCookieName, DateTime.Now.AddMinutes(Session.Timeout), false);
                }
            }
            user = AuthenticationHelper.AuthenticateUser(loginEmail, password, SiteContext.CurrentSiteName);
            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.UserName, isKeepMeLoggedIn);
                MembershipActivityLogger.LogLogin(user.UserName);
                return new LogonUserResultDTO { success = true };
            }
            else
            {
                return new LogonUserResultDTO { success = false, errorPropertyName = "loginEmail", errorMessage = ResHelper.GetString("Kadena.Logon.LogonFailed", LocalizationContext.CurrentCulture.CultureCode) };
            }
        }

        private GeneralResultDTO InitialPasswordSettingInternal(string password, string confirmPassword, Guid userGUID)
        {
            var ui = UserInfoProvider.GetUserInfoByGUID(userGUID);
            if (ui == null)
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.InitialPasswordSetting.PasswordCantBeSetUp", LocalizationContext.CurrentCulture.CultureCode) };
            }
            UserInfoProvider.SetPassword(ui, password);
            ui.Enabled = true;
            ui.Update();

            return new GeneralResultDTO { success = true };
        }

        private GeneralResultDTO ContactPersonDetailsChangeInternal(Guid userGUID, string firstName, string lastName, string mobile, string phoneNumber)
        {
            var ui = UserInfoProvider.GetUserInfoByGUID(userGUID);
            if (ui == null)
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.ContanctDetailsChange.UserNotFound", LocalizationContext.CurrentCulture.CultureCode) };
            }
            ui.FirstName = firstName;
            ui.LastName = lastName;
            ui.FullName = firstName + " " + lastName;
            ui.SetValue("UserMobile", mobile);
            ui.SetValue("UserPhone", phoneNumber);

            ui.Update();

            return new GeneralResultDTO { success = true };
        }

        private GeneralResultDTO ChangePasswordInternal(Guid userGUID, string oldPassword, string newPassword)
        {
            var ui = UserInfoProvider.GetUserInfoByGUID(userGUID);
            if (ui == null)
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.Settings.Password.UserNotFound", LocalizationContext.CurrentCulture.CultureCode) };
            }
            if (!UserInfoProvider.ValidateUserPassword(ui, oldPassword))
            {
                return new GeneralResultDTO { success = false, errorMessage = ResHelper.GetString("Kadena.Settings.Password.OldPasswordIsNotValid", LocalizationContext.CurrentCulture.CultureCode) };
            }
            UserInfoProvider.SetPassword(ui, newPassword);
            ui.Update();

            return new GeneralResultDTO { success = true };
        }

        private bool IsEmailValid(string email)
        {
            var regexText = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                            + "@"
                            + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

            Regex regex = new Regex(regexText);
            Match match = regex.Match(email);
            return match.Success;
        }

        #endregion
    }
}
