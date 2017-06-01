namespace CMSApp
{
  using CMS.Activities.Loggers;
  using CMS.Helpers;
  using CMS.Localization;
  using CMS.Membership;
  using CMS.SiteProvider;
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
    #region Public classes

    /// <summary>
    /// container for passing result of logon service call
    /// </summary>
    public class LogonUserResult
    {
      public bool success { get; set; }
      public string errorMessage { get; set; }
      public string errorPropertyName { get; set; }
    }

    public class GeneralResult
    {
      public bool success { get; set; }
      public string errorMessage { get; set; }
    }

    #endregion

    #region Public methods

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public void SignOut()
    {
      AuthenticationHelper.SignOut();
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public LogonUserResult LogonUser(string loginEmail, string password, bool isKeepMeLoggedIn)
    {
      #region Validation

      if (string.IsNullOrWhiteSpace(loginEmail))
      {
        return new LogonUserResult { success = false, errorPropertyName = "loginEmail", errorMessage = ResHelper.GetString("Kadena.Logon.LoginEmailEmpty", LocalizationContext.CurrentCulture.CultureCode) };
      }
      if (string.IsNullOrWhiteSpace(password))
      {
        return new LogonUserResult { success = false, errorPropertyName = "password", errorMessage = ResHelper.GetString("Kadena.Logon.PasswordEmpty", LocalizationContext.CurrentCulture.CultureCode) };
      }
      if (!IsEmailValid(loginEmail))
      {
        return new LogonUserResult { success = false, errorPropertyName = "loginEmail", errorMessage = ResHelper.GetString("Kadena.Logon.InvalidEmail", LocalizationContext.CurrentCulture.CultureCode) };
      }

      #endregion

      return LogonUserInternal(loginEmail, password, isKeepMeLoggedIn);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public GeneralResult InitialPasswordSetting(string password, string confirmPassword, Guid userGUID)
    {
      #region Validation

      if (string.IsNullOrWhiteSpace(password))
      {
        return new GeneralResult { success = false, errorMessage = ResHelper.GetString("Kadena.InitialPasswordSetting.PasswordIsEmpty", LocalizationContext.CurrentCulture.CultureCode) };
      }
      if (string.IsNullOrWhiteSpace(confirmPassword))
      {
        return new GeneralResult { success = false, errorMessage = ResHelper.GetString("Kadena.InitialPasswordSetting.PasswordIsEmpty", LocalizationContext.CurrentCulture.CultureCode) };
      }
      if (password != confirmPassword)
      {
        return new GeneralResult { success = false, errorMessage = ResHelper.GetString("Kadena.InitialPasswordSetting.PasswordsAreNotTheSame", LocalizationContext.CurrentCulture.CultureCode) };
      }

      #endregion

      return InitialPasswordSettingInternal(password, confirmPassword, userGUID);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public GeneralResult ContactPersonDetailsChange(Guid userGUID, string firstName, string lastName, string mobile, string phoneNumber)
    {
      return ContactPersonDetailsChangeInternal(userGUID, firstName, lastName, mobile, phoneNumber);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public GeneralResult ChangePassword(Guid userGUID, string oldPassword, string newPassword, string confirmPassword)
    {
      #region Validation

      if (string.IsNullOrEmpty(oldPassword))
      {
        return new GeneralResult { success = false, errorMessage = ResHelper.GetString("Kadena.Settings.Password.OldPasswordIsEmpty", LocalizationContext.CurrentCulture.CultureCode) };
      }
      if (string.IsNullOrEmpty(newPassword))
      {
        return new GeneralResult { success = false, errorMessage = ResHelper.GetString("Kadena.Settings.Password.NewPasswordIsEmpty", LocalizationContext.CurrentCulture.CultureCode) };
      }
      if (string.IsNullOrEmpty(confirmPassword))
      {
        return new GeneralResult { success = false, errorMessage = ResHelper.GetString("Kadena.Settings.Password.ConfirmPasswordIsEmpty", LocalizationContext.CurrentCulture.CultureCode) };
      }
      if (newPassword != confirmPassword)
      {
        return new GeneralResult { success = false, errorMessage = ResHelper.GetString("Kadena.Settings.Password.PasswordsDontMatch", LocalizationContext.CurrentCulture.CultureCode) };
      }

      #endregion

      return ChangePasswordInternal(userGUID, oldPassword, newPassword);
    }

    #endregion

    #region Private methods

    private LogonUserResult LogonUserInternal(string loginEmail, string password, bool isKeepMeLoggedIn)
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
        return new LogonUserResult { success = true };
      }
      else
      {
        return new LogonUserResult { success = false, errorPropertyName = "loginEmail", errorMessage = ResHelper.GetString("Kadena.Logon.LogonFailed", LocalizationContext.CurrentCulture.CultureCode) };
      }
    }

    private GeneralResult InitialPasswordSettingInternal(string password, string confirmPassword, Guid userGUID)
    {
      var ui = UserInfoProvider.GetUserInfoByGUID(userGUID);
      if (ui == null)
      {
        return new GeneralResult { success = false, errorMessage = ResHelper.GetString("Kadena.InitialPasswordSetting.PasswordCantBeSetUp", LocalizationContext.CurrentCulture.CultureCode) };
      }
      UserInfoProvider.SetPassword(ui, password);
      ui.Enabled = true;
      ui.Update();

      return new GeneralResult { success = true };
    }

    private GeneralResult ContactPersonDetailsChangeInternal(Guid userGUID, string firstName, string lastName, string mobile, string phoneNumber)
    {
      var ui = UserInfoProvider.GetUserInfoByGUID(userGUID);
      if (ui == null)
      {
        return new GeneralResult { success = false, errorMessage = ResHelper.GetString("Kadena.ContanctDetailsChange.UserNotFound", LocalizationContext.CurrentCulture.CultureCode) };
      }
      ui.FirstName = firstName;
      ui.LastName = lastName;
      ui.FullName = firstName + " " + lastName;
      ui.SetValue("UserMobile", mobile);
      ui.SetValue("UserPhone", phoneNumber);

      ui.Update();

      return new GeneralResult { success = true };
    }

    private GeneralResult ChangePasswordInternal(Guid userGUID, string oldPassword, string newPassword)
    {
      var ui = UserInfoProvider.GetUserInfoByGUID(userGUID);
      if (ui == null)
      {
        return new GeneralResult { success = false, errorMessage = ResHelper.GetString("Kadena.Settings.Password.UserNotFound", LocalizationContext.CurrentCulture.CultureCode) };
      }
      if (!UserInfoProvider.ValidateUserPassword(ui, oldPassword))
      {
        return new GeneralResult { success = false, errorMessage = ResHelper.GetString("Kadena.Settings.Password.OldPasswordIsNotValid", LocalizationContext.CurrentCulture.CultureCode) };
      }
      UserInfoProvider.SetPassword(ui, newPassword);
      ui.Update();

      return new GeneralResult { success = true };
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
