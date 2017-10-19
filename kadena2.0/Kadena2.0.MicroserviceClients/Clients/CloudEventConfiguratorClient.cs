using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Threading.Tasks;
using Kadena.Dto.General;
using System.Net.Http;
using Newtonsoft.Json;
using Kadena2.MicroserviceClients.Clients.Base;

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

        public async Task<BaseResponseDto<string>> UpdateNooshRule(string endPoint, string ruleName, bool enabled, int rate, string targetId, string workGroupName, string nooshUrl, string nooshToken)
        {
            using (var client = new HttpClient())
            {
                using (var content = CreateRequestContent(new
                {
                    RuleName = ruleName,
                    Rate = rate,
                    Enabled = enabled,
                    TargetId = targetId,
                    TargetType = TargetType.Noosh,
                    InputParameters = new
                    {
                        WorkGroups = new string[] { workGroupName },
                        NooshUrl = nooshUrl,
                        NooshToken = nooshToken
                    }
                }))
                {
                    using (var response = await client.PutAsync($"{endPoint}/cloudwatch", content).ConfigureAwait(false))
                    {
                        return await ReadResponseJson<string>(response);
                    }
                }
            }
        }
    }
}
