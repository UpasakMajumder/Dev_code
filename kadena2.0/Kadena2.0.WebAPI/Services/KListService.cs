using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Kadena.WebAPI.Services
{
    public class KListService : IKListService
    {
        private readonly IMailingListClient _client;
        private readonly IKenticoResourceService _kentico;

        public KListService(IMailingListClient client, IKenticoResourceService kenticoResource)
        {
            _client = client;
            _kentico = kenticoResource;
        }

        public async Task<bool> UseOnlyCorrectAddresses(Guid containerId)
        {
            var getAddressUrl = _kentico.GetSettingsKey("KDA_GetMailingAddressesUrl");
            var removeAddressesUrl = _kentico.GetSettingsKey("KDA_DeleteAddressesUrl");
            var validateUrl = _kentico.GetSettingsKey("KDA_ValidateAddressUrl");
            var customerName = _kentico.GetKenticoSite().Name;

            var addresses = await _client.GetAddresses(getAddressUrl, containerId);
            await _client.RemoveAddresses(removeAddressesUrl, customerName, containerId, addresses.Payload
                .Where(a=>a.Error != null)
                .Select(a => a.Id));
            var validateResult = await _client.Validate(removeAddressesUrl, customerName, containerId);

            return validateResult.Success;
        }
    }
}