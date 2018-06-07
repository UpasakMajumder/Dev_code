using System;
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
        public ParsingClient(IMicroProperties properties)
        {
            _serviceUrlSettingKey = "KDA_ParsingServiceUrl";
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public async Task<BaseResponseDto<IEnumerable<string>>> GetHeaders(string fileId)
        {
            var url = $"{BaseUrlOld}/api/CsvParser/GetHeaders?FileId={fileId}&Module={FileModule.KList}";
            return await Get<IEnumerable<string>>(url).ConfigureAwait(false);
        }
    }
}
