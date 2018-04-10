using Kadena2.MicroserviceClients.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena.Dto.KSource;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class BidClient : SignedClientBase, IBidClient
    {
        public BidClient(IMicroProperties properties)
        {
            _serviceUrlSettingKey = "KDA_BidServiceUrl";
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public async Task<BaseResponseDto<IEnumerable<ProjectDto>>> GetProjects(string workGroupName)
        {
            var url = $"{BaseUrl}/api/bid/{workGroupName}";
            return await Get<IEnumerable<ProjectDto>>(url).ConfigureAwait(false);
        }
    }
}
