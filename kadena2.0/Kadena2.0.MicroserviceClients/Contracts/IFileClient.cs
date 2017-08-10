using Kadena.Dto.General;
using System.IO;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IFileClient
    {
        Task<BaseResponseDto<string>> UploadToS3(string serviceEndpoint, string siteName, string folderName, string moduleName,
            Stream fileStream, string fileName);

        string GetFileUrl(string serviceEndpoint, string fileName, string moduleName);
    }
}
