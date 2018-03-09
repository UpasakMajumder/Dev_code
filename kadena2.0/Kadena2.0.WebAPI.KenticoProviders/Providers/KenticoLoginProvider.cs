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
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoLoginProvider : IKenticoLoginProvider
    {
        private readonly IKenticoLogger logger;
        private readonly IKadenaSettings kadenaSettings;

        public KenticoLoginProvider(IKenticoLogger logger, IKadenaSettings kadenaSettings)
        {
            this.logger = logger;
            this.kadenaSettings = kadenaSettings;
        }

        public DateTime GetTaCValidFrom()
        {
            var tacNodeAliasPath = kadenaSettings.TermsAndConditionsPage;
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

        public bool CheckPassword(string mail, string password)
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
            var user = AuthenticationHelper.AuthenticateUser(loginRequest.LoginEmail, loginRequest.Password, SiteContext.CurrentSiteName);

            if (user != null)
            {
                ChangeCookieExpiration(loginRequest.KeepLoggedIn);
                FormsAuthentication.SetAuthCookie(user.UserName, loginRequest.KeepLoggedIn);
                MembershipActivityLogger.LogLogin(user.UserName);

                return new LoginResult
                {
                    LogonSuccess = true
                };
            }
            
            return new LoginResult
            {
                LogonSuccess = false,
                ErrorPropertyName = "loginEmail",
                ErrorMessage = ResHelper.GetString("Kadena.Logon.LogonFailed", LocalizationContext.CurrentCulture.CultureCode)
            };
        }

        public bool SSOLogin(string username, bool keepLoggedIn = false)
        {
            var user = UserInfoProvider.GetUserInfo(username);

            if (user == null)
            {
                return false;
            }

            AuthenticationHelper.AuthenticateUser(username, createPersistentCookie: true, loadCultures: true);

            var authenticationSuccess = (MembershipContext.AuthenticatedUser?.UserID ?? 0) == user.UserID;

            if (authenticationSuccess)
            {
                ChangeCookieExpiration(keepLoggedIn);
                FormsAuthentication.SetAuthCookie(username, keepLoggedIn);
                MembershipActivityLogger.LogLogin(username);
            }

            return authenticationSuccess;
        }

        private void ChangeCookieExpiration(bool keepLoggedIn)
        {
            CookieHelper.EnsureResponseCookie(FormsAuthentication.FormsCookieName);

            if (keepLoggedIn)
            {
                var extendedExpirationDate = DateTime.Now.AddYears(1);
                CookieHelper.ChangeCookieExpiration(FormsAuthentication.FormsCookieName, extendedExpirationDate, false);
            }
            else
            {
                // Extend the expiration of the authentication cookie if required
                if (!AuthenticationHelper.UseSessionCookies && (HttpContext.Current?.Session != null))
                {
                    var extendedExpirationDate = DateTime.Now.AddMinutes(HttpContext.Current.Session.Timeout);
                    CookieHelper.ChangeCookieExpiration(FormsAuthentication.FormsCookieName, extendedExpirationDate, false);
                }
            }
        }

        public void Logout()
        {
            AuthenticationHelper.SignOut();
        }
    }
}