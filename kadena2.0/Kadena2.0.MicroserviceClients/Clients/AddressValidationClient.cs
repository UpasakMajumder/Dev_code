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
        public AddressValidationClient(IMicroProperties properties) 
        {
            _serviceUrlSettingKey = "KDA_ValidateAddressUrl";
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public async Task<BaseResponseDto<string>> Validate(Guid containerId)
        {
            var url = $"{BaseUrlOld}/api/AddressValidator/";
            return await Post<string>(url, new
            {
                ContainerId = containerId,
                CustomerName = _properties.GetCustomerName()
            }).ConfigureAwait(false);
        }
    }
}
