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
        public FileClient(IMicroProperties properties)
        {
            _serviceUrlSettingKey = "KDA_FileServiceUrl";
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public async Task<BaseResponseDto<string>> GetShortliveSecureLink(string key)
        {
            var encodedKey = HttpUtility.UrlEncode(key);
            var url = $"{BaseUrlOld}/api/filelink?key={encodedKey}";
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
            return await Post<string>(url, body).ConfigureAwait(false);
        }
    }
}
