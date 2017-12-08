using Kadena.Models;
using Kadena.Models.Login;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ILoginService
    {
        CheckTaCResult CheckTaC(LoginRequest request);
        void AcceptTaC(LoginRequest request);
        bool UserHasAcceptedTac(User user);
        LoginResult Login(LoginRequest request);
    }
}