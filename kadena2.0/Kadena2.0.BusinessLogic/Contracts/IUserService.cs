using Kadena.Models;
using Kadena.Models.Login;
using Kadena.Models.Membership;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IUserService
    {
        CheckTaCResult CheckTaC();
        void AcceptTaC();
        void RegisterUser(Registration registration);
    }
}
