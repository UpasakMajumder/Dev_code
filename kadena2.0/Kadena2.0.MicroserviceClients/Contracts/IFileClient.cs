using Kadena.Dto.General;
using System.IO;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IFileClient
    {
        Task<BaseResponseDto<string>> GetShortliveSecureLink(string key);

        Task<BaseResponseDto<string>> GetFileKey(FileSystem system, FileType fileType, string siteName, string fileName, string extension);
    }
}
