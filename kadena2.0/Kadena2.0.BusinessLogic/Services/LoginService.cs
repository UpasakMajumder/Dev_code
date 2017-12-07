using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.Login;
using System;

namespace Kadena.BusinessLogic.Services
{
    public class LoginService : ILoginService
    {
        private readonly IKenticoProviderService _kentico;
        private readonly IKenticoUserProvider _kenticoUsers;
        private readonly IKenticoResourceService _resources;

        public LoginService(IKenticoProviderService kentico, IKenticoUserProvider kenticoUsers, IKenticoResourceService resources)
        {
            //todo null checks

            _kentico = kentico;
            _kenticoUsers = kenticoUsers;
            _resources = resources;
        }

        public CheckTaCResult CheckTaC(LoginRequest request)
        {
            var user = _kenticoUsers.GetUser(request.LoginEmail);
            return null;
        }

        public LoginResult Login(LoginRequest request)
        {
            throw new NotImplementedException();
        }
    }
}