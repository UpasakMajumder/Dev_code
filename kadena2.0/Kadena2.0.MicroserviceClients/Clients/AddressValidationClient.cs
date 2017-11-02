using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Clients.Base;

namespace Kadena2.MicroserviceClients.Clients
{
    public class AddressValidationClient : ClientBase, IAddressValidationClient
    {
        public async Task<BaseResponseDto<string>> Validate(string endPoint, string customerName, Guid containerId)
        {
            var url = $"{endPoint}/api/AddressValidator/";
            return await Post<string>(url, new
            {
                ContainerId = containerId,
                CustomerName = customerName
            }).ConfigureAwait(false);
        }
    }
}
