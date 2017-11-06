using Kadena.Dto.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface ICloudEventConfiguratorClient
    {
        Task<BaseResponseDto<string>> UpdateNooshRule(string ruleName, bool enabled, int rate
            , string targetId, string workGroupName, string nooshUrl, string nooshToken);
    }
}
