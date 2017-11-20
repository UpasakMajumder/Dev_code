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
    public class ExportClient : ClientBase, IExportClient
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

            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                await SignRequestMessage(request).ConfigureAwait(false);

                using (var client = new HttpClient())
                {
                    using (var message = await client.SendAsync(request).ConfigureAwait(false))
                    {
                        if (message.IsSuccessStatusCode)
                        {
                            var contentStream = await message.Content.ReadAsStreamAsync();
                            var resultStream = new MemoryStream();
                            await contentStream.CopyToAsync(resultStream).ConfigureAwait(false);
                            return new BaseResponseDto<Stream>
                            {
                                Success = true,
                                Payload = resultStream
                            };
                        }
                        else
                        {
                            return await ReadResponseJson<Stream>(message).ConfigureAwait(false);
                        }
                    }
                }
            }
        }
    }
}
