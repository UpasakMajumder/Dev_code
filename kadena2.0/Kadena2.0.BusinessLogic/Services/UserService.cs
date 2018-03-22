using Kadena.BusinessLogic.Contracts;
using Kadena.Models;
using Kadena.Models.Login;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Security;

namespace Kadena.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoDocumentProvider documents;
        private readonly IKenticoLoginProvider login;

        public UserService(IKenticoUserProvider kenticoUsers, IKenticoResourceService resources, IKenticoDocumentProvider documents, IKenticoLoginProvider login)
        {
            this.kenticoUsers = kenticoUsers ?? throw new ArgumentNullException(nameof(kenticoUsers));
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.documents = documents ?? throw new ArgumentNullException(nameof(documents));
            this.login = login ?? throw new ArgumentNullException(nameof(login));
        }

        public CheckTaCResult CheckTaC(LoginRequest request)
        {
            if (!login.CheckPassword(request.LoginEmail, request.Password))
            {
                return CheckTaCResult.GetFailedResult("loginEmail", resources.GetResourceString("Kadena.Logon.LogonFailed"));
            }

            var tacEnabled = resources.GetSettingsKey("KDA_TermsAndConditionsLogin").ToLower() == "true";

            var showTaC = false;

            if (tacEnabled)
            {
                var user = kenticoUsers.GetUser(request.LoginEmail);
                showTaC = !UserHasAcceptedTac(user);
            }

            return new CheckTaCResult
            {
                LogonSuccess = true,
                ShowTaC = showTaC,
                Url = showTaC ? GetTacPageUrl() : string.Empty
            };
        }

        private bool UserHasAcceptedTac(User user)
        {
            var tacValidFrom = documents.GetTaCValidFrom();
            return user.TermsConditionsAccepted > tacValidFrom;
        }

        public void AcceptTaC(LoginRequest request)
        {
            if (!login.CheckPassword(request.LoginEmail, request.Password))
            {
                throw new SecurityException("Invalid username or password");
            }

            login.AcceptTaC(request.LoginEmail);
        }

        private string GetTacPageUrl()
        {
            var tacAliasPath = resources.GetSettingsKey("KDA_TermsAndConditionPage");
            return documents.GetDocumentUrl(tacAliasPath);
        }
    }
}
