using CMS.DocumentEngine;
using CMS.Localization;
using CMS.Membership;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using Kadena.Models.Login;
using CMS.Helpers;
using System.Web.Security;
using System.Web;
using CMS.SiteProvider;
using CMS.Activities.Loggers;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoLoginProvider : IKenticoLoginProvider
    {
        private readonly IKenticoLogger logger;
        private readonly IKenticoResourceService resource;

        public KenticoLoginProvider(IKenticoLogger logger, IKenticoResourceService resource)
        {
            this.logger = logger;
            this.resource = resource;
        }

        public DateTime GetTaCValidFrom()
        {
            var tacNodeAliasPath = resource.GetSettingsKey("KDA_TermsAndConditionPage");
            var tacDocument = DocumentHelper.GetDocuments("KDA.TermsAndConditions")
                .OnCurrentSite()
                .Path(tacNodeAliasPath)
                .Culture(LocalizationContext.CurrentUICulture.CultureCode)
                .FirstObject;

            if (tacDocument == null)
            {
                throw new Exception("Unable to find Terms and condition page");
            }

            return tacDocument.GetDateTimeValue("ValidFrom", DateTime.MinValue);
        }

        public bool CheckPasword(string mail, string password)
        {
            var user = UserInfoProvider.GetUserInfo(mail);

            if(user == null)
            {
                return false;
            }

            return !UserInfoProvider.IsUserPasswordDifferent(user, password);
       }

        public void AcceptTaC(string mail)
        {
            var user = UserInfoProvider.GetUserInfo(mail);
            user.SetValue("TermsConditionsAccepted", DateTime.Now);
            user.Update();
        }

        public LoginResult Login(LoginRequest loginRequest)
        {
            CookieHelper.EnsureResponseCookie(FormsAuthentication.FormsCookieName);
            if (loginRequest.KeepLoggedIn)
            {
                CookieHelper.ChangeCookieExpiration(FormsAuthentication.FormsCookieName, DateTime.Now.AddYears(1), false);
            }
            else
            {
                // Extend the expiration of the authentication cookie if required
                if (!AuthenticationHelper.UseSessionCookies && (HttpContext.Current != null) && (HttpContext.Current.Session != null))
                {
                    CookieHelper.ChangeCookieExpiration(FormsAuthentication.FormsCookieName, DateTime.Now.AddMinutes(HttpContext.Current.Session.Timeout), false);
                }
            }

            var user = AuthenticationHelper.AuthenticateUser(loginRequest.LoginEmail, loginRequest.Password, SiteContext.CurrentSiteName);

            if (user != null)
            {
                FormsAuthentication.SetAuthCookie(user.UserName, loginRequest.KeepLoggedIn);
                MembershipActivityLogger.LogLogin(user.UserName);
                return new LoginResult
                {
                    LogonSuccess = true
                };
            }
            else
            {
                return new LoginResult
                {
                    LogonSuccess = false,
                    ErrorPropertyName = "loginEmail",
                    ErrorMessage = ResHelper.GetString("Kadena.Logon.LogonFailed", LocalizationContext.CurrentCulture.CultureCode)
                };
            }
        }
    }
}