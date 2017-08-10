using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Threading.Tasks;
using Kadena.Dto.General;
using System.IO;
using System.Web;
using Kadena2.MicroserviceClients.Clients.Base;
using System.Net.Http;

namespace Kadena2.MicroserviceClients.Clients
{
    public class FileClient : ClientBase, IFileClient
    {
        public string GetFileUrl(string serviceEndpoint, string fileName, string moduleName)
        {
            return $"{serviceEndpoint}/GetFileStreamBy?key={HttpUtility.UrlEncode(fileName)}&module={moduleName}";
        }

        public async Task<BaseResponseDto<string>> UploadToS3(string serviceEndpoint, string siteName, string folderName, string moduleName, Stream fileStream, string fileName)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    fileStream.Seek(0, SeekOrigin.Begin);
                    content.Add(new StreamContent(fileStream), "file", fileName);
                    content.Add(new StringContent(folderName), "ConsumerDetails.BucketType");
                    content.Add(new StringContent(siteName), "ConsumerDetails.CustomerName");
                    content.Add(new StringContent(moduleName), "ConsumerDetails.Module");
                    using (var message = await client.PostAsync(serviceEndpoint, content).ConfigureAwait(false))
                    {
                        return await ReadResponseJson<string>(message);
                    }
                }
            }
        }
    }
}
