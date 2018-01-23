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
    public sealed class FileClient : SignedClientBase, IFileClient
    {
        private const string _serviceUrlSettingKey = "KDA_FileServiceUrl";
        private readonly IMicroProperties _properties;

        private string _siteName;
        private FileFolder _fileFolder;
        private FileModule _moduleName;
        private Stream _fileStream;
        private string _fileName;

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
            _siteName = siteName;
            _fileFolder = folderName;
            _moduleName = moduleName;
            _fileStream = fileStream;
            _fileName = fileName;

            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            url = $"{url}/api/File";
            return await Post<string>(url, null).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<string>> GetShortliveSecureLink(string key, FileModule module)
        {
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            url = $"{url}/api/File?key={key}&module={module}";
            return await Get<string>(url);
        }

        protected override HttpRequestMessage CreateRequest(HttpMethod method, string url, object body = null)
        {
            var request = base.CreateRequest(method, url, body);
            if (_fileStream != null)
            {
                var content = new MultipartFormDataContent();
                _fileStream.Seek(0, SeekOrigin.Begin);
                content.Add(new StreamContent(_fileStream), "file", _fileName);
                content.Add(new StringContent(_fileFolder.ToString()), "ConsumerDetails.BucketType");
                content.Add(new StringContent(_siteName), "ConsumerDetails.CustomerName");
                content.Add(new StringContent(_moduleName.ToString()), "ConsumerDetails.Module");
                request.Content = content;
            }
            return request;
        }

        public async Task<BaseResponseDto<string>> GetShortliveSecureLink(string key)
        {
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            url = $"{url}/api/File?key={key}";
            return await Get<string>(url);
        }
    }
}
