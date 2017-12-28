using Kadena.Dto.General;
using System.IO;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IFileClient
    {
        /// <summary>
        /// Uploads file to S3 bucket with request to microservice.
        /// </summary>
        /// <param name="fileStream">Stream to upload.</param>
        /// <param name="fileName">Name of file to pass to microservice.</param>
        /// <returns>Id of uploaded file.</returns>
        Task<BaseResponseDto<string>> UploadToS3(string siteName, FileFolder folderName,
            FileModule moduleName, Stream fileStream, string fileName);

        string GetFileUrl(string fileName, FileModule moduleName);

        Task<BaseResponseDto<string>> GetShortliveSecureLink(string key, FileModule module);
    }
}
