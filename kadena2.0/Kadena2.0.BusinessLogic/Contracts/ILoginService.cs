using Kadena.Models.Login;
using Kadena.Models.Membership;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ILoginService
    {
        CheckTaCResult CheckTaC(LoginRequest request);
        void AcceptTaC(LoginRequest request);
        bool UserHasAcceptedTac(User user);
        LoginResult Login(LoginRequest request);
        string Logout();
    }
}