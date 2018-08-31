using Kadena.Dto.General;
using Kadena.Dto.Membership;
using Kadena.Models.SiteSettings;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public class UserManagerClient : SignedClientBase, IUserManagerClient
    {
        public UserManagerClient(IMicroProperties properties)
        {
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
            _serviceVersionSettingKey = Settings.KDA_UserManagerVersion;
        }

        public async Task<BaseResponseDto<object>> Create(CreateUserDto user)
        {
            var url = $"{BaseUrl}/user/v{Version}";
            return await Post<object>(url, user).ConfigureAwait(false);
        }
    }
}
