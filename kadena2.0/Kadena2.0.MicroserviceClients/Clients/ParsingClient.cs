using System.Collections.Generic;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class ParsingClient : SignedClientBase, IParsingClient
    {
        private const string _serviceUrlSettingKey = "KDA_ParsingServiceUrl";
        private readonly IMicroProperties _properties;

        public ParsingClient(IMicroProperties properties)
        {
            _properties = properties;
        }

        public async Task<BaseResponseDto<IEnumerable<string>>> GetHeaders(string fileId)
        {
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            url = $"{url}/api/CsvParser/GetHeaders?FileId={fileId}&Module={FileModule.KList}";
            return await Get<IEnumerable<string>>(url).ConfigureAwait(false);
        }
    }
}
