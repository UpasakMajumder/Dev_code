using Kadena.Dto.CreditCard.MicroserviceRequests;
using Kadena.Dto.General;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IUserDataServiceClient
    {
        Task<BaseResponseDto<string>> SaveCardToken(SaveCardTokenRequestDto request);
    }
}
