using System;
using System.IO;
using System.Threading.Tasks;
using Kadena.AmazonFileSystemProvider;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients;
using Kadena2.MicroserviceClients.Contracts;

namespace Kadena.BusinessLogic.Services
{
    public class FileService : IFileService
    {
        private readonly IFileClient _fileClient;
        private readonly IExportClient exportClient;
        private readonly IKenticoResourceService _resources;
        private readonly IKenticoLogger _logger;
        private readonly IS3PathService pathService;
        private readonly IKenticoFileProvider fileProvider;
        private readonly IKenticoSiteProvider siteProvider;

        public FileService(IFileClient fileClient, IKenticoResourceService resources, IKenticoLogger logger, IS3PathService pathService,
            IKenticoFileProvider fileProvider, IExportClient exportClient, IKenticoSiteProvider siteProvider)
        {
            _fileClient = fileClient ?? throw new ArgumentNullException(nameof(fileClient));
            _resources = resources ?? throw new ArgumentNullException(nameof(resources));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.pathService = pathService ?? throw new ArgumentNullException(nameof(pathService));
            this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
            this.exportClient = exportClient ?? throw new ArgumentNullException(nameof(exportClient));
            this.siteProvider = siteProvider ?? throw new ArgumentNullException(nameof(siteProvider));
        }

        public string CreateMailingList(string fileName, Stream fileStream)
        {
            var system = FileSystem.Mailing;
            var path = Path.Combine(system.SystemFolder, fileName);
            fileProvider.CreateFile(path, fileStream);
            return pathService.GetObjectKeyFromPath(path, true);
        }

        public async Task<Uri> GetContainerFileUrl(Guid containerId)
        {
            var site = siteProvider.GetCurrentSiteCodeName();
            var exportResult = await exportClient.ExportMailingList(containerId);
            if (!exportResult.Success)
            {
                _logger.LogError(GetType().Name, exportResult.ErrorMessages);
                return null;
            }
            return new Uri(exportResult.Payload, UriKind.RelativeOrAbsolute);
        }

        public async Task<string> GetUrlFromS3(string key)
        {
            var linkResult = await _fileClient.GetShortliveSecureLink(pathService.GetObjectKeyFromPath(key, true));

            if (!linkResult.Success || string.IsNullOrEmpty(linkResult.Payload))
            {
                _logger.LogError("GetUrlFromS3", "Failed to get link for file from S3.");
                return null;
            }

            return linkResult.Payload;
        }
    }
}
