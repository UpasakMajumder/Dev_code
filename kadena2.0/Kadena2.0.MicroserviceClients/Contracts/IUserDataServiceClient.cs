using Kadena.Dto.CreditCard.MicroserviceRequests;
using Kadena.Dto.CreditCard.MicroserviceResponses;
using Kadena.Dto.General;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IUserDataServiceClient
    {
        Task<BaseResponseDto<SaveCardTokenResponseDto>> SaveCardToken(string serviceEndpoint, SaveCardTokenRequestDto request);
    }
}
