using Kadena.Models.Login;
using Kadena.Models.Membership;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ILoginService
    {
        LoginResult Login(LoginRequest request);
        string Logout();
    }
}