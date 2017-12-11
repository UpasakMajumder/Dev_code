using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.Login;
using System;
using System.Security;
using Kadena.Models;

namespace Kadena.BusinessLogic.Services
{
    public class LoginService : ILoginService
    {
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoDocumentProvider documents;
        private readonly IKenticoLoginProvider login;

        public LoginService(IKenticoUserProvider kenticoUsers, IKenticoResourceService resources, IKenticoDocumentProvider documents, IKenticoLoginProvider login)
        {
            if (kenticoUsers == null)
            {
                throw new ArgumentNullException(nameof(kenticoUsers));
            }
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }
            if (documents == null)
            {
                throw new ArgumentNullException(nameof(documents));
            }
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            this.kenticoUsers = kenticoUsers;
            this.resources = resources;
            this.documents = documents;
            this.login = login;
        }

        public CheckTaCResult CheckTaC(LoginRequest request)
        {
            var user = kenticoUsers.GetUser(request.LoginEmail);

            if (!login.CheckPasword(request.LoginEmail, request.Password))
            {
                throw new SecurityException("Invalid username or password");
            }

            var tacEnabled = resources.GetSettingsKey("KDA_TermsAndConditionsLogin").ToLower() == "true";
            var userHasAccepted = UserHasAcceptedTac(user);

            var showTaC = tacEnabled && !userHasAccepted;

            return new CheckTaCResult
            {
                ShowTaC = showTaC,
                Url = showTaC ? GetTacPageUrl() : string.Empty
            };
        }

        public bool UserHasAcceptedTac(User user)
        {
            var tacValidFrom = login.GetTaCValidFrom();
            return user.TermsConditionsAccepted > tacValidFrom;
        }

        public void AcceptTaC(LoginRequest request)
        {
            if (!login.CheckPasword(request.LoginEmail, request.Password))
            {
                throw new SecurityException("Invalid username or password");
            }

            login.AcceptTaC(request.LoginEmail);
        }

        public LoginResult Login(LoginRequest request)
        {
            var validation = request.Validate();
            if (validation != null)
            {
                return GetFailedLoginResult(validation.Name,  resources.GetResourceString(validation.Error));
            }

            var user = kenticoUsers.GetUser(request.LoginEmail);
            if (user == null || !kenticoUsers.UserIsInCurrentSite(user.UserId))
            {
                return GetFailedLoginResult("loginEmail", resources.GetResourceString("Kadena.Logon.LogonFailed"));
            }

            var tacEnabled = resources.GetSettingsKey("KDA_TermsAndConditionsLogin").ToLower() == "true";
            if (tacEnabled && !UserHasAcceptedTac(user))
            {
                return GetFailedLoginResult("loginEmail", resources.GetResourceString("Kadena.Logon.LogonFailed"));
            }

            return login.Login(request);
        }

        private string GetTacPageUrl()
        {
            var tacAliasPath = resources.GetSettingsKey("KDA_TermsAndConditionPage");
            return documents.GetDocumentUrl(tacAliasPath);
        }

        private LoginResult GetFailedLoginResult(string property, string error)
        {
            return new LoginResult
            {
                LogonSuccess = false,
                ErrorPropertyName = property,
                ErrorMessage = error
            };
        }
    }
}