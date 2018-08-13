using AutoMapper;
using Kadena.Dto.General;
using Kadena.Dto.Membership;
using Kadena.Models.Membership;
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
        private readonly IMapper mapper;

        public UserManagerClient(IMicroProperties properties, IMapper mapper)
        {
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _serviceVersionSettingKey = Settings.KDA_UserManagerVersion;
        }

        public async Task<BaseResponseDto<object>> Create(User user)
        {
            var url = $"{BaseUrl}/user";
            var body = mapper.Map<CreateUserDto>(user);
            return await Post<object>(url, body).ConfigureAwait(false);
        }
    }
}
