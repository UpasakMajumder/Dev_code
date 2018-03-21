using Kadena.Models;
using Kadena.Models.Login;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ILoginService
    {
        LoginResult Login(LoginRequest request);
        string Logout();
    }
}