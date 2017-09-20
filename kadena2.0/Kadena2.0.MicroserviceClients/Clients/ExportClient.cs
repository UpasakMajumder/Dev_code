using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
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
                using (var message = await client.GetAsync(url).ConfigureAwait(false))
                {
                    if (message.IsSuccessStatusCode)
                    {
                        var contentStream = await message.Content.ReadAsStreamAsync();
                        var resultStream = new MemoryStream();
                        await contentStream.CopyToAsync(resultStream);
                        return new BaseResponseDto<Stream>
                        {
                            Success = true,
                            Payload = resultStream
                        };
                    }
                    else
                    {
                        return await ReadResponseJson<Stream>(message);
                    }
                }
            }
        }
    }
}
