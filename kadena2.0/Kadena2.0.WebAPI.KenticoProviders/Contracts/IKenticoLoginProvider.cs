using Kadena.Models;
using Kadena.Models.Login;
using System;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoLoginProvider
    {
        DateTime GetTaCValidFrom();
        bool CheckPasword(string mail, string password);
        void AcceptTaC(string mail);
        LoginResult Login(LoginRequest loginRequest);
    }
}
