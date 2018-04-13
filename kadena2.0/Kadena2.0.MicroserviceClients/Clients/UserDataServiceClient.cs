using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena.Dto.CreditCard.MicroserviceRequests;
using Kadena.Dto.CreditCard.MicroserviceResponses;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;
using System.Collections.Generic;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class UserDataServiceClient : SignedClientBase, IUserDataServiceClient
    {
        public UserDataServiceClient(IMicroProperties properties)
        {
            _serviceUrlSettingKey = "KDA_UserDataServiceUrl";
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public async Task<BaseResponseDto<string>> SaveCardToken(SaveCardTokenRequestDto request)
        {
            var url = $"{BaseUrlOld}/api/CardToken";
            return await Post<string>(url, request).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<IEnumerable<UserStoredCardDto>>> GetValidCardTokens(int userId)
        {
            var url = $"{BaseUrlOld}/api/CardToken/valid/{userId}";
            return await Get<IEnumerable<UserStoredCardDto>>(url).ConfigureAwait(false);
        }
    }
}

