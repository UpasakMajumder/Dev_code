using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using Kadena.Dto.General;
using System.IO;
using System.Web;
using Kadena2.MicroserviceClients.Clients.Base;
using System.Net.Http;
using Kadena2.MicroserviceClients.Contracts.Base;
using System;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class FileClient : SignedClientBase, IFileClient
    {
        private string _siteName;
        private FileFolder _fileFolder;
        private FileModule _moduleName;
        private Stream _fileStream;
        private string _fileName;

        public FileClient(IMicroProperties properties)
        {
            _serviceUrlSettingKey = "KDA_FileServiceUrl";
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public string GetFileUrl(string fileName, FileModule moduleName)
        {
            return $"{BaseUrlOld}/api/File/GetFileStreamBy?key={HttpUtility.UrlEncode(fileName)}&module={moduleName}";
        }

        public async Task<BaseResponseDto<string>> UploadToS3(string siteName, FileFolder folderName,
            FileModule moduleName, Stream fileStream, string fileName)
        {
            _siteName = siteName;
            _fileFolder = folderName;
            _moduleName = moduleName;
            _fileStream = fileStream;
            _fileName = fileName;

            var url = $"{BaseUrlOld}/api/File";
            return await Post<string>(url, null).ConfigureAwait(false);
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
            var encodedKey = HttpUtility.UrlEncode(key);
            var url = $"{BaseUrlOld}/api/File?key={encodedKey}";
            return await Get<string>(url);
        }

        public async Task<BaseResponseDto<string>> GetFileKey(FileSystem system, FileType fileType, string siteName, string fileName, string extension)
        {
            var url = $"{BaseUrlOld}/api/filelink";
            var body = new
            {
                System = system.ToString(),
                Filetype = fileType.ToString(),
                client = siteName,
                FileName = fileName,
                FileExt = extension
            };
            return await Post<string>(url, body);
        }
    }
}
