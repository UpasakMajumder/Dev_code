using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.Dto.General;
using System.Net.Http;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts.Base;
using Kadena.Dto.KSource;

namespace Kadena2.MicroserviceClients.Clients
{
    public class CloudEventConfiguratorClient : ClientBase, ICloudEventConfiguratorClient
    {
        enum TargetType
        {
            Stream = 4,
            GateWayApi = 5,
            Lambda = 6,
            Noosh = 12
        }

        private const string _serviceUrlSettingKey = "KDA_CloudEventConfiguratorUrl";
        private readonly IMicroProperties _properties;

        public CloudEventConfiguratorClient(IMicroProperties properties)
        {
            _properties = properties;
        }

        public async Task<BaseResponseDto<string>> UpdateNooshRule(RuleDto rule, NooshDto noosh)
        {
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            url = $"{url}/cloudwatch";
            var body = new
            {
                RuleName = rule.RuleName,
                Rate = rule.Rate,
                Enabled = rule.Enabled,
                TargetId = rule.TargetId,
                TargetType = TargetType.Noosh,
                InputParameters = new
                {
                    WorkGroups = new string[] { noosh.WorkgroupName },
                    NooshUrl = noosh.NooshUrl,
                    NooshToken = noosh.NooshToken
                }
            };

            return await Send<string>(HttpMethod.Put, url, body).ConfigureAwait(false);
        }
    }
}
