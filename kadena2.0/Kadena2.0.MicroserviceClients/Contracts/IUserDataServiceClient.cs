using Kadena.Dto.CreditCard.MicroserviceRequests;
using Kadena.Dto.CreditCard.MicroserviceResponses;
using Kadena.Dto.General;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IUserDataServiceClient
    {
        Task<BaseResponseDto<string>> SaveCardToken(SaveCardTokenRequestDto request);
        Task<BaseResponseDto<IEnumerable<UserStoredCardDto>>> GetValidCardTokens(int userId);
    }
}
