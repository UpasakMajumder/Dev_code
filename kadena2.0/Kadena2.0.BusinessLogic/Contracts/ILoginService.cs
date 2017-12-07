using Kadena.Models.Login;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ILoginService
    {
        CheckTaCResult CheckTaC(LoginRequest request);
        LoginResult Login(LoginRequest request);
    }
}