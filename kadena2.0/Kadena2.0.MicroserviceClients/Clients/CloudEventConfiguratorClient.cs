using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts.Base;
using Kadena.Dto.KSource;
using System;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class CloudEventConfiguratorClient : SignedClientBase, ICloudEventConfiguratorClient
    {
        enum TargetType
        {
            Stream = 4,
            GateWayApi = 5,
            Lambda = 6,
            Noosh = 12
        }

        public CloudEventConfiguratorClient(IMicroProperties properties)
        {
            _serviceUrlSettingKey = "KDA_CloudEventConfiguratorUrl";
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public async Task<BaseResponseDto<string>> UpdateNooshRule(RuleDto rule, NooshDto noosh)
        {
            var url = $"{BaseUrlOld}/cloudwatch";
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

            return await Put<string>(url, body).ConfigureAwait(false);
        }
    }
}
