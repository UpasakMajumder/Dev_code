using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;
using Kadena.Models;
using System.Collections.Generic;
using AutoMapper;
using Kadena.Dto.MailingList.MicroserviceResponses;

namespace Kadena.WebAPI.Services
{
    public class KListService : IKListService
    {
        private readonly IMailingListClient _mailingClient;
        private readonly IAddressValidationClient _validationClient;
        private readonly IKenticoResourceService _kentico;
        private readonly IMapper _mapper;
        private readonly string _mailingServiceUrlSettingKey = "KDA_MailingServiceUrl";

        public KListService(IMailingListClient client, IKenticoResourceService kenticoResource, IAddressValidationClient validationClient, IMapper mapper)
        {
            _mailingClient = client;
            _kentico = kenticoResource;
            _mapper = mapper;
            _validationClient = validationClient;
        }

        public async Task<MailingList> GetMailingList(Guid containerId)
        {
            var mailingServiceUrl = _kentico.GetSettingsKey(_mailingServiceUrlSettingKey);
            var customerName = _kentico.GetKenticoSite().Name;

            var data = await _mailingClient.GetMailingList(mailingServiceUrl, customerName, containerId);
            return _mapper.Map<MailingList>(data.Payload);
        }

        public async Task<bool> UpdateAddresses(Guid containerId, IEnumerable<MailingAddress> addresses)
        {
            var mailingServiceUrl = _kentico.GetSettingsKey(_mailingServiceUrlSettingKey);
            var customerName = _kentico.GetKenticoSite().Name;
            var changes = _mapper.Map<MailingAddressDto[]>(addresses);

            var updateResult = await _mailingClient.UpdateAddresses(mailingServiceUrl, customerName, containerId, changes);
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
            var mailingServiceUrl = _kentico.GetSettingsKey(_mailingServiceUrlSettingKey);
            var customerName = _kentico.GetKenticoSite().Name;

            var addresses = await _mailingClient.GetAddresses(mailingServiceUrl, containerId);
            await _mailingClient.RemoveAddresses(mailingServiceUrl, customerName, containerId,
                addresses.Payload.Where(a => a.ErrorMessage != null).Select(a => a.Id));
            var validateResult = await _validationClient.Validate(containerId);

            return validateResult.Success;
        }
    }
}