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
        public async Task<BaseResponseDto<object>> UpdateNooshRule(string endPoint, string ruleName, bool enabled, int rate, string targetId, string workGroupName, string nooshUrl, string nooshToken)
        {
            using (var client = new HttpClient())
            {
                using (var content = new StringContent(JsonConvert.SerializeObject(
                    new
                    {
                        RuleName = ruleName,
                        Rate = rate,
                        TargetId = targetId,
                        TargetType = 12,
                        InputParameters = new
                        {
                            WorkGroups = new string[] { workGroupName },
                            NooshUrl = nooshUrl,
                            nooshToken = nooshToken
                        }
                    }
                    ), System.Text.Encoding.UTF8, "application/json"))
                {
                    using (var response = await client.PutAsync($"{endPoint}/cloudwatch", content).ConfigureAwait(false))
                    {
                        return await ReadResponseJson<object>(response);
                    }
                }
            }
        }
    }
}
