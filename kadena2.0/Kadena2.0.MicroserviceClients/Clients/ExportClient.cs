using Kadena.Dto.General;
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
        private const string _serviceUrlSettingKey = "KDA_ExportServiceUrl";
        private readonly IMicroProperties _properties;

        public ExportClient(IMicroProperties properties)
        {
            _properties = properties;
        }

        public async Task<BaseResponseDto<Stream>> ExportMailingList(Guid containerId, string siteName)
        {
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            url = $"{url}/api/MailingListExport/GetFileReport?ContainerId={containerId}&SiteName={siteName}&ReportType=processedMails&OutputType=csv";
            return await Get<Stream>(url).ConfigureAwait(false);
        }

        protected override async Task<BaseResponseDto<TResult>> ReadResponseJson<TResult>(HttpResponseMessage response)
        {
            if (typeof(TResult).Equals(typeof(Stream)) && response.IsSuccessStatusCode)
            {
                var contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var resultStream = new MemoryStream();
                await contentStream.CopyToAsync(resultStream).ConfigureAwait(false);
                return new BaseResponseDto<TResult>
                {
                    Success = true,
                    Payload = (TResult)(object)resultStream
                };
            }
            else
            {
                return await base.ReadResponseJson<TResult>(response).ConfigureAwait(false);
            }
        }
    }
}
