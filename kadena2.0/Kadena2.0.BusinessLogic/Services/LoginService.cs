using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.Login;
using System;

namespace Kadena.BusinessLogic.Services
{
    public class LoginService : ILoginService
    {
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoResourceService resources;
        private readonly IKenticoLoginProvider login;
        private readonly IUserService userService;

        public LoginService(IKenticoUserProvider kenticoUsers, IKenticoResourceService resources, IKenticoLoginProvider login, IUserService userService)
        {
            this.kenticoUsers = kenticoUsers ?? throw new ArgumentNullException(nameof(kenticoUsers));
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.login = login ?? throw new ArgumentNullException(nameof(login));
            this.userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public LoginResult Login(LoginRequest request)
        {
            var validation = request.Validate();
            if (validation != null)
            {
                return GetFailedLoginResult(validation.Name, resources.GetResourceString(validation.Error));
            }

            var user = kenticoUsers.GetUser(request.LoginEmail);
            if (user == null || !kenticoUsers.UserIsInCurrentSite(user.UserId))
            {
                return GetFailedLoginResult("loginEmail", resources.GetResourceString("Kadena.Logon.LogonFailed"));
            }

            var tacEnabled = resources.GetSettingsKey("KDA_TermsAndConditionsLogin").ToLower() == "true";
            if (tacEnabled && !userService.UserHasAcceptedTac(user))
            {
                return GetFailedLoginResult("loginEmail", resources.GetResourceString("Kadena.Logon.LogonFailed"));
            }

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

        public string Logout()
        {
            login.Logout();
            string redirectUrl = "/"; // TODO get SAML value here somehow, make parameter for it
            return redirectUrl;
        }
    }
}