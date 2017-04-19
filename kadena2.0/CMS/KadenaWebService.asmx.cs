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
