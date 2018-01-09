using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;

namespace Kadena.BusinessLogic.Services
{
    public class FileService : IFileService
    {
        private readonly IFileClient _fileClient;
        private readonly IKenticoResourceService _resources;
        private readonly IKenticoLogger _logger;

        public FileService(IFileClient fileClient, IKenticoResourceService resources, IKenticoLogger logger)
        {
            _fileClient = fileClient ?? throw new ArgumentNullException(nameof(fileClient));
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> GetUrlFromS3(string key)
        {
            var module = _resources.GetSettingsKey("KDA_AmazonS3BucketName");
            var linkResult = await _fileClient.GetShortliveSecureLink(key, module);

            if (!linkResult.Success || string.IsNullOrEmpty(linkResult.Payload))
            {
                _logger.LogError("GetUrlFromS3", "Failed to get link for file from S3.");
                return null;
            }

            return linkResult.Payload;
        }
    }
}
