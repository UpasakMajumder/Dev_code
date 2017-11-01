using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Threading.Tasks;
using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Clients.Base;

namespace Kadena2.MicroserviceClients.Clients
{
    public class AddressValidationClient : ClientBase, IAddressValidationService
    {
        public async Task<BaseResponseDto<string>> Validate(string endPoint, string customerName, Guid containerId)
        {
            return await Post<string>(endPoint, new
            {
                ContainerId = containerId,
                CustomerName = customerName
            }).ConfigureAwait(false);
        }
    }
}
