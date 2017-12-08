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
        private readonly IKenticoProviderService kentico;
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoDocumentProvider documents;
        private readonly IKenticoLoginProvider login;

        public LoginService(IKenticoProviderService kentico, IKenticoUserProvider kenticoUsers, IKenticoResourceService resources, IKenticoDocumentProvider documents, IKenticoLoginProvider login)
        {
            if (kentico == null)
            {
                throw new ArgumentNullException(nameof(kentico));
            }
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

            this.kentico = kentico;
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

            var userHasAccepted = UserHasAcceptedTac(user);

            return new CheckTaCResult
            {
                ShowTaC = !userHasAccepted,
                Url = userHasAccepted ? GetTacPageUrl() : string.Empty
            };
        }

        private string GetTacPageUrl()
        {
            var tacAliasPath = resources.GetSettingsKey("KDA_TermsAndConditionPage");
            return documents.GetDocumentUrl(tacAliasPath);
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

            return login.Login(request);
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