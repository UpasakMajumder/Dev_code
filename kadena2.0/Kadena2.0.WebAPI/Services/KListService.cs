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
        private readonly IMailingListClient _client;
        private readonly IKenticoResourceService _kentico;
        private readonly IMapper _mapper;

        public KListService(IMailingListClient client, IKenticoResourceService kenticoResource, IMapper mapper)
        {
            _client = client;
            _kentico = kenticoResource;
            _mapper = mapper;
        }

        public async Task<bool> UpdateAddresses(Guid containerId, IEnumerable<MailingAddress> addresses)
        {
            var updateUrl = _kentico.GetSettingsKey("KDA_UpdateAddressesUrl");
            var customerName = _kentico.GetKenticoSite().Name;
            var validateUrl = _kentico.GetSettingsKey("KDA_ValidateAddressUrl");
            var changes = _mapper.Map<MailingAddressDto[]>(addresses);

            var updateResult = await _client.UpdateAddresses(updateUrl, customerName, containerId, changes);
            if (updateResult.Success)
            {
                var validateResult = await _client.Validate(validateUrl, customerName, containerId);
                return validateResult.Success;
            }
            else
            {
                return updateResult.Success;
            }
        }

        public async Task<bool> UseOnlyCorrectAddresses(Guid containerId)
        {
            var getAddressUrl = _kentico.GetSettingsKey("KDA_GetMailingAddressesUrl");
            var removeAddressesUrl = _kentico.GetSettingsKey("KDA_DeleteAddressesUrl");
            var validateUrl = _kentico.GetSettingsKey("KDA_ValidateAddressUrl");
            var customerName = _kentico.GetKenticoSite().Name;

            var addresses = await _client.GetAddresses(getAddressUrl, containerId);
            await _client.RemoveAddresses(removeAddressesUrl, customerName, containerId,
                addresses.Payload.Where(a => a.Error != null).Select(a => a.Id));
            var validateResult = await _client.Validate(validateUrl, customerName, containerId);

            return validateResult.Success;
        }
    }
}