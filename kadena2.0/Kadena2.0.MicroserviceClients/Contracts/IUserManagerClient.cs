using Kadena.Models.Membership;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IUserManagerClient
    {
        bool Create(User user);
    }
}
