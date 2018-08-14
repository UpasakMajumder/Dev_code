using Kadena.Dto.General;
using Kadena.Dto.Membership;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IUserManagerClient
    {
        Task<BaseResponseDto<object>> Create(CreateUserDto user);
    }
}
