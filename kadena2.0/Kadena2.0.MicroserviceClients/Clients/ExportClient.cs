using Kadena.Dto.General;
using Kadena.Models.SiteSettings;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class ExportClient : SignedClientBase, IExportClient
    {
        public ExportClient(IMicroProperties properties)
        {
            _serviceVersionSettingKey = Settings.KDA_ExportServiceVersion;
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public async Task<BaseResponseDto<string>> ExportMailingList(Guid containerId, string siteName)
        {
            var url = $"{BaseUrl}/export/mailinglist?containerId={containerId}&siteName={siteName}&reportType=processedMails&outputType=csv";
            return await Get<string>(url).ConfigureAwait(false);
        }
    }
}
