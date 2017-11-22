using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts.Base;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class AddressValidationClient : SignedClientBase, IAddressValidationClient
    {
        private const string _serviceUrlSettingKey = "KDA_ValidateAddressUrl";
        private readonly IMicroProperties _properties;

        public AddressValidationClient(IMicroProperties properties)
        {
            _properties = properties;
        }

        public async Task<BaseResponseDto<string>> Validate(Guid containerId)
        {
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            url = $"{url}/api/AddressValidator/";
            return await Post<string>(url, new
            {
                ContainerId = containerId,
                CustomerName = _properties.GetCustomerName()
            }).ConfigureAwait(false);
        }
    }
}
