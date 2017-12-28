using Kadena2.MicroserviceClients.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena.Dto.KSource;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts.Base;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class BidClient : SignedClientBase, IBidClient
    {
        private const string _serviceUrlSettingKey = "KDA_BidServiceUrl";
        private readonly IMicroProperties _properties;

        public BidClient(IMicroProperties properties)
        {
            _properties = properties;
        }

        public async Task<BaseResponseDto<IEnumerable<ProjectDto>>> GetProjects(string workGroupName)
        {
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            url = $"{url}/api/bid/{workGroupName}";
            return await Get<IEnumerable<ProjectDto>>(url).ConfigureAwait(false);
        }
    }
}
