using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena.Models.SiteSettings;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class ParsingClient : SignedClientBase, IParsingClient
    {
        public ParsingClient(IMicroProperties properties)
        {
            _serviceVersionSettingKey = Settings.KDA_ParsingServiceVersion;
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public async Task<BaseResponseDto<IEnumerable<string>>> GetHeaders(string fileId)
        {
            var url = $"{BaseUrl}/parser/headers";
            var body = new
            {
                fileId
            };
            return await Post<IEnumerable<string>>(url, body).ConfigureAwait(false);
        }
    }
}
