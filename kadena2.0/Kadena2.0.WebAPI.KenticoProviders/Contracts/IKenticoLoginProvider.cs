using Kadena.Models.Login;
using System;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoLoginProvider
    {
        bool CheckPassword(string mail, string password);
        void AcceptTaC(string mail);
        LoginResult Login(LoginRequest loginRequest);
        bool SSOLogin(string userName, bool keepLoggedIn);
        void Logout();
    }
}
