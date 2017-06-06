namespace CMSApp
{
  using CMS.Activities.Loggers;
  using CMS.DataEngine;
  using CMS.EmailEngine;
  using CMS.Helpers;
  using CMS.Localization;
  using CMS.MacroEngine;
  using CMS.Membership;
  using CMS.OnlineForms;
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

    #region Static members

    public const string _ForgottenPasswordFormCodeName = "KDA_ForgottenPasswordForm";
    public const string _RequestAccessFormCodeName = "KDA_RequestAccessForm";
    public const string _ContactUsFormCodeName = "KDA_ContactUsForm";

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
      if (!SecurityHelper.CheckPasswordPolicy(password, SiteContext.CurrentSiteName))
      {
        return new GeneralResult { success = false, errorMessage = AuthenticationHelper.GetPolicyViolationMessage(SiteContext.CurrentSiteName) };
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
      if (!SecurityHelper.CheckPasswordPolicy(newPassword, SiteContext.CurrentSiteName))
      {
        return new GeneralResult { success = false, errorMessage = AuthenticationHelper.GetPolicyViolationMessage(SiteContext.CurrentSiteName) };
      }

      #endregion

      return ChangePasswordInternal(userGUID, oldPassword, newPassword);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public GeneralResult SubmitContactUsForm(string fullName, string companyName, string email, string phone, string message)
    {
      #region Validation

      if (string.IsNullOrWhiteSpace(fullName))
      {
        return new GeneralResult { success = false, errorMessage = ResHelper.GetString("Kadena.ContactForm.EnterYourFullName", LocalizationContext.CurrentCulture.CultureCode) };
      }
      if (!string.IsNullOrWhiteSpace(email) && !IsEmailValid(email))
      {
        return new GeneralResult { success = false, errorMessage = ResHelper.GetString("Kadena.ContactForm.EmailAddressIsNotValid", LocalizationContext.CurrentCulture.CultureCode) };
      }
      if (string.IsNullOrWhiteSpace(message))
      {
        return new GeneralResult { success = false, errorMessage = ResHelper.GetString("Kadena.ContactForm.EnterYourRequest", LocalizationContext.CurrentCulture.CultureCode) };
      }

      #endregion

      return SubmitContactUsFormInternal(fullName, companyName, email, phone, message);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public GeneralResult SubmitForgottenPasswordForm(string email)
    {
      #region Validation

      if (string.IsNullOrWhiteSpace(email))
      {
        return new GeneralResult { success = false, errorMessage = ResHelper.GetString("Kadena.ForgottenPassword.EmailIsEmpty", LocalizationContext.CurrentCulture.CultureCode) };
      }
      if (!IsEmailValid(email))
      {
        return new GeneralResult { success = false, errorMessage = ResHelper.GetString("Kadena.ForgottenPassword.EmailIsNotValid", LocalizationContext.CurrentCulture.CultureCode) };
      }

      #endregion

      return SubmitForgottenPasswordFormInternal(email);
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod]
    public GeneralResult SubmitRequestAccessForm(string email)
    {
      #region Validation

      if (string.IsNullOrWhiteSpace(email))
      {
        return new GeneralResult { success = false, errorMessage = ResHelper.GetString("Kadena.RequestAccess.EmailIsEmpty", LocalizationContext.CurrentCulture.CultureCode) };
      }
      if (!IsEmailValid(email))
      {
        return new GeneralResult { success = false, errorMessage = ResHelper.GetString("Kadena.RequestAccess.EmailIsNotValid", LocalizationContext.CurrentCulture.CultureCode) };
      }

      #endregion

      return SubmitRequestAccessFormInternal(email);
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

    private GeneralResult SubmitForgottenPasswordFormInternal(string email)
    {
      var forgottenPasswordForm = BizFormInfoProvider.GetBizFormInfo(_ForgottenPasswordFormCodeName, SiteContext.CurrentSiteID);
      if (forgottenPasswordForm != null)
      {
        var forgottenPasswordFormClass = DataClassInfoProvider.GetDataClassInfo(forgottenPasswordForm.FormClassID);
        string forgottenPasswordFormClassName = forgottenPasswordFormClass.ClassName;

        BizFormItem newFormItem = BizFormItem.New(forgottenPasswordFormClassName);

        newFormItem.SetValue("Email", email);
        newFormItem.SetValue("Site", SiteContext.CurrentSite.DisplayName);
        newFormItem.SetValue("FormInserted", DateTime.Now);
        newFormItem.SetValue("FormUpdated", DateTime.Now);

        newFormItem.Insert();

        SendFormEmail(newFormItem);

        return new GeneralResult { success = true };
      }
      else
      {
        return new GeneralResult { success = false, errorMessage = ResHelper.GetString("Kadena.ForgottenPassword.ForgottenPasswordRepositoryNotFound", LocalizationContext.CurrentCulture.CultureCode) };
      }
    }

    private GeneralResult SubmitRequestAccessFormInternal(string email)
    {
      var requestAccessForm = BizFormInfoProvider.GetBizFormInfo(_RequestAccessFormCodeName, SiteContext.CurrentSiteID);
      if (requestAccessForm != null)
      {
        var requestAccessFormClass = DataClassInfoProvider.GetDataClassInfo(requestAccessForm.FormClassID);
        string requestAccessFormClassName = requestAccessFormClass.ClassName;

        BizFormItem newFormItem = BizFormItem.New(requestAccessFormClassName);

        newFormItem.SetValue("Email", email);
        newFormItem.SetValue("Site", SiteContext.CurrentSite.DisplayName);
        newFormItem.SetValue("FormInserted", DateTime.Now);
        newFormItem.SetValue("FormUpdated", DateTime.Now);

        newFormItem.Insert();

        SendFormEmail(newFormItem);

        return new GeneralResult { success = true };
      }
      else
      {
        return new GeneralResult { success = false, errorMessage = ResHelper.GetString("Kadena.ForgottenPassword.ForgottenPasswordRepositoryNotFound", LocalizationContext.CurrentCulture.CultureCode) };
      }
    }

    public GeneralResult SubmitContactUsFormInternal(string fullName, string companyName, string email, string phone, string message)
    {
      var contactUsForm = BizFormInfoProvider.GetBizFormInfo(_ContactUsFormCodeName, SiteContext.CurrentSiteID);
      if (contactUsForm != null)
      {
        var contactUsFormClass = DataClassInfoProvider.GetDataClassInfo(contactUsForm.FormClassID);
        string contactUsFormClassName = contactUsFormClass.ClassName;

        BizFormItem newFormItem = BizFormItem.New(contactUsFormClassName);

        newFormItem.SetValue("FullName", fullName);
        newFormItem.SetValue("CompanyName", companyName);
        newFormItem.SetValue("Email", email);
        newFormItem.SetValue("Phone", phone);
        newFormItem.SetValue("Message", message);
        newFormItem.SetValue("Site", SiteContext.CurrentSite.DisplayName);
        newFormItem.SetValue("FormInserted", DateTime.Now);
        newFormItem.SetValue("FormUpdated", DateTime.Now);

        newFormItem.Insert();

        SendFormEmail(newFormItem);

        return new GeneralResult { success = true };
      }
      else
      {
        return new GeneralResult { success = false, errorMessage = ResHelper.GetString("Kadena.ContactForm.ContactFormRepositoryNotFound", LocalizationContext.CurrentCulture.CultureCode) };
      }
    }

    private void SendFormEmail(BizFormItem item)
    {
      if (item.BizFormInfo != null)
      {
        MacroResolver resolver = MacroContext.CurrentResolver.CreateChild();
        resolver.SetAnonymousSourceData(item);
        resolver.Settings.EncodeResolvedValues = true;
        resolver.Culture = CultureHelper.GetPreferredCulture();

        string body = DataHelper.GetNotEmpty(item.BizFormInfo.FormEmailTemplate, string.Empty);
        body = resolver.ResolveMacros(body);

        EmailMessage message = new EmailMessage();
        message.From = item.BizFormInfo.FormSendFromEmail;
        message.Recipients = resolver.ResolveMacros(item.BizFormInfo.FormSendToEmail);
        message.Subject = resolver.ResolveMacros(item.BizFormInfo.FormEmailSubject);
        message.Body = URLHelper.MakeLinksAbsolute(body);
        message.EmailFormat = EmailFormatEnum.Html;

        EmailSender.SendEmail(message);
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
