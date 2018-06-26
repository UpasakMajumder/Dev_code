using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;
using Kadena.Models;
using System.Collections.Generic;
using AutoMapper;
using Kadena.Dto.MailingList.MicroserviceResponses;
using Kadena.Models.SiteSettings;
using Kadena2.MicroserviceClients;
using System.IO;
using Kadena.AmazonFileSystemProvider;

namespace Kadena.BusinessLogic.Services
{
    public class KListService : IKListService
    {
        private readonly IMailingListClient _mailingClient;
        private readonly IKenticoSiteProvider _site;
        private readonly IKenticoResourceService _resourceService;
        private readonly IMapper _mapper;
        private readonly IKenticoFileProvider fileProvider;
        private readonly IS3PathService pathService;
        private readonly IExportClient exportClient;
        private readonly IKenticoLogger _logger;

        public KListService(IMailingListClient client, IKenticoSiteProvider site, IMapper mapper,
            IKenticoResourceService resourceService, IKenticoFileProvider fileProvider, IExportClient exportClient,
            IS3PathService pathService, IKenticoLogger logger)
        {
            _mailingClient = client ?? throw new ArgumentNullException(nameof(client));
            _site = site ?? throw new ArgumentNullException(nameof(site));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
            this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
            this.exportClient = exportClient ?? throw new ArgumentNullException(nameof(exportClient));
            this.pathService = pathService ?? throw new ArgumentNullException(nameof(pathService));
            this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> DeleteExpiredMailingLists()
        {
            var site = _site.GetKenticoSite();
            var expirationDaysStr = _resourceService.GetSettingsKey<string>(Settings.KDA_MailingList_DeleteExpiredAfter);
            if (int.TryParse(expirationDaysStr, out int expirationDays))
            {
                var deleteOlderThan = DateTime.Today.AddDays(-expirationDays);
                var result = await _mailingClient.RemoveMailingList(deleteOlderThan).ConfigureAwait(false);

                if (!result.Success)
                {
                    return $"Failure for {site} - {result.ErrorMessages}.";
                }
                return $"{site} - Done.";
            }

            return $"{site} - Setting not set. Skipping.";
        }

        public async Task<MailingList> GetMailingList(Guid containerId)
        {
            var customerName = _site.GetKenticoSite().Name;

            var data = await _mailingClient.GetMailingList(containerId);
            return _mapper.Map<MailingList>(data.Payload);
        }

        public async Task<bool> UpdateAddresses(Guid containerId, IEnumerable<MailingAddress> addresses)
        {
            var changes = _mapper.Map<MailingAddressDto[]>(addresses);

            var updateResult = await _mailingClient.UpdateAddresses(containerId, changes);
            return updateResult.Success;
        }

        public async Task<bool> UseOnlyCorrectAddresses(Guid containerId)
        {
            var addresses = await _mailingClient.GetAddresses(containerId);
            var removeResult = await _mailingClient.RemoveAddresses(containerId,
                addresses.Payload.Where(a => a.ErrorMessage != null).Select(a => a.Id));

            return removeResult.Success;
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
            var site = _site.GetCurrentSiteCodeName();
            var exportResult = await exportClient.ExportMailingList(containerId);
            if (!exportResult.Success)
            {
                _logger.LogError(GetType().Name, exportResult.ErrorMessages);
                return null;
            }
            return new Uri(exportResult.Payload, UriKind.RelativeOrAbsolute);
        }
    }
}