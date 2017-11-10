using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Clients
{
    public class ExportClient : ClientBase, IExportClient
    {
        public async Task<BaseResponseDto<Stream>> ExportMailingList(string endpoint, Guid containerId, string siteName)
        {
            var url = $"{endpoint}/api/MailingListExport/GetFileReport?ContainerId={containerId}&SiteName={siteName}&ReportType=processedMails&OutputType=csv";
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    if (SignRequest)
                    {
                        await SignRequestMessage(request).ConfigureAwait(false);
                    }

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
