using Kadena2.MicroserviceClients.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena.Dto.KSource;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts.Base;

namespace Kadena2.MicroserviceClients.Clients
{
    public class BidClient : ClientBase, IBidClient
    {
        //public BidClient() : base()
        //{

        //}

        //public BidClient(IAwsV4Signer signer) : base(signer)
        //{

        //}

        private const string _serviceUrlSettingKey = "KDA_BidServiceUrl";
        private readonly IMicroProperties _properties;

        public BidClient(IMicroProperties properties)
        {
            _properties = properties;
        }

        public async Task<BaseResponseDto<IEnumerable<ProjectDto>>> GetProjects(string endPoint, string workGroupName)
        {
            var url = $"{endPoint}/api/bid/{workGroupName}";
            return await Get<IEnumerable<ProjectDto>>(url).ConfigureAwait(false);
        }
    }
}
