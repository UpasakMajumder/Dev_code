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

namespace Kadena.BusinessLogic.Services
{
    public class KListService : IKListService
    {
        private readonly IMailingListClient _mailingClient;
        private readonly IAddressValidationClient _validationClient;
        private readonly IKenticoSiteProvider _site;
        private readonly IKenticoResourceService _resourceService;
        private readonly IMapper _mapper;

        public KListService(IMailingListClient client, IKenticoSiteProvider site, IAddressValidationClient validationClient, IMapper mapper,
            IKenticoResourceService resourceService)
        {
            _mailingClient = client ?? throw new ArgumentNullException(nameof(client));
            _site = site ?? throw new ArgumentNullException(nameof(site));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _validationClient = validationClient ?? throw new ArgumentNullException(nameof(validationClient));
            _resourceService = resourceService ?? throw new ArgumentNullException(nameof(resourceService));
        }

        public async Task<string> DeleteExpiredMailingLists()
        {
            var expirationDays = _resourceService.GetSettingsKey<int>(Settings.KDA_MailingList_DeleteExpiredAfter);
            var deleteOlderThan = DateTime.Now.AddDays(-expirationDays);
            var result = await _mailingClient.RemoveMailingList(deleteOlderThan).ConfigureAwait(false);

            var site = _site.GetKenticoSite();
            if (!result.Success)
            {
                return $"Failure for {site} - {result.ErrorMessages}";
            }

            return $"{site} - Done";
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
            if (updateResult.Success)
            {
                var validateResult = await _validationClient.Validate(containerId);
                return validateResult.Success;
            }
            else
            {
                return updateResult.Success;
            }
        }

        public async Task<bool> UseOnlyCorrectAddresses(Guid containerId)
        {
            var addresses = await _mailingClient.GetAddresses(containerId);
            await _mailingClient.RemoveAddresses(containerId,
                addresses.Payload.Where(a => a.ErrorMessage != null).Select(a => a.Id));
            var validateResult = await _validationClient.Validate(containerId);

            return validateResult.Success;
        }
    }
}