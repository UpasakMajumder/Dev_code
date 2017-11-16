using Kadena.Dto.General;
using Kadena.Dto.KSource;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface ICloudEventConfiguratorClient
    {
        Task<BaseResponseDto<string>> UpdateNooshRule(RuleDto rule, NooshDto noosh);
    }
}
