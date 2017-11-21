using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.Dto.General;
using System.IO;
using System.Web;
using Kadena2.MicroserviceClients.Clients.Base;
using System.Net.Http;
using Kadena2.MicroserviceClients.Contracts.Base;

namespace Kadena2.MicroserviceClients.Clients
{
    public class FileClient : ClientBase, IFileClient
    {
        private const string _serviceUrlSettingKey = "KDA_FileServiceUrl";
        private readonly IMicroProperties _properties;

        public FileClient(IMicroProperties properties)
        {
            _properties = properties;
        }

        public string GetFileUrl(string fileName, FileModule moduleName)
        {
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            return $"{url}/api/File/GetFileStreamBy?key={HttpUtility.UrlEncode(fileName)}&module={moduleName}";
        }

        public async Task<BaseResponseDto<string>> UploadToS3(string siteName, FileFolder folderName,
            FileModule moduleName, Stream fileStream, string fileName)
        {
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            url = $"{url}/api/File";
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    var content = new MultipartFormDataContent();
                    fileStream.Seek(0, SeekOrigin.Begin);
                    content.Add(new StreamContent(fileStream), "file", fileName);
                    content.Add(new StringContent(folderName.ToString()), "ConsumerDetails.BucketType");
                    content.Add(new StringContent(siteName), "ConsumerDetails.CustomerName");
                    content.Add(new StringContent(moduleName.ToString()), "ConsumerDetails.Module");
                    request.Content = content;

                    using (var message = await client.SendAsync(request).ConfigureAwait(false))
                    {
                        return await ReadResponseJson<string>(message).ConfigureAwait(false);
                    }
                }
            }
        }
    }
}
