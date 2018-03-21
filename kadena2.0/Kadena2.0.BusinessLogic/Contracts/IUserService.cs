using Kadena.Models;
using Kadena.Models.Login;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IUserService
    {
        CheckTaCResult CheckTaC(LoginRequest request);
        void AcceptTaC(LoginRequest request);
        bool UserHasAcceptedTac(User user);
    }
}
