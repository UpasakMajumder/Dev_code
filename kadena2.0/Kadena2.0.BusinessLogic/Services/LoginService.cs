using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.Login;
using System;
using System.Security;

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

            var tacValidFrom = login.GetTaCValidFrom();
            var result = new CheckTaCResult();

            if (user.TermsConditionsAccepted < tacValidFrom)
            {
                var tacAliasPath = resources.GetSettingsKey("KDA_TermsAndConditionPage");
                var tacUrl = documents.GetDocumentUrl(tacAliasPath);
                result.ShowTaC = true;
                result.Url = tacUrl;
            }

            return result;
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
            // todo validations in model

            return login.Login(request);
        }
    }
}