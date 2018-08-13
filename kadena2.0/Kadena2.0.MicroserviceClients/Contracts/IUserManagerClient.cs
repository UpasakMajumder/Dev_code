using Kadena.Dto.General;
using Kadena.Models.Membership;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IUserManagerClient
    {
        Task<BaseResponseDto<object>> Create(User user);
    }
}
