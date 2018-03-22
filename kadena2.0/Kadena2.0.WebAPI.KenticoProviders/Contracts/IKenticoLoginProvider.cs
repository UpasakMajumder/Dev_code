using Kadena.Models.Login;
using System;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoLoginProvider
    {
        bool CheckPassword(string mail, string password);
        LoginResult Login(LoginRequest loginRequest);
        bool SSOLogin(string userName, bool keepLoggedIn);
        void Logout();
    }
}
