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

        public LoginService(IKenticoUserProvider kenticoUsers, IKenticoResourceService resources, IKenticoLoginProvider login)
        {
            this.kenticoUsers = kenticoUsers ?? throw new ArgumentNullException(nameof(kenticoUsers));
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.login = login ?? throw new ArgumentNullException(nameof(login));
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
            var user = kenticoUsers.GetCurrentUser();
            login.Logout();
            return string.IsNullOrWhiteSpace(user?.CallBackUrl) ? "/" : user.CallBackUrl;
        }
    }
}