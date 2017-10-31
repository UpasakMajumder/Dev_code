using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kadena.Dto.General;
using System.Net.Http;
using Kadena2.MicroserviceClients.Clients.Base;

namespace Kadena2.MicroserviceClients.Clients
{
    public class AddressValidationClient : ClientBase, IAddressValidationService
    {
        public async Task<BaseResponseDto<string>> Validate(string endPoint, string customerName, Guid containerId)
        {
            using (var client = new HttpClient())
            {
                using (var content = CreateRequestContent(new
                {
                    ContainerId = containerId,
                    CustomerName = customerName
                }))
                {
                    using (var message = await client.PostAsync(endPoint, content).ConfigureAwait(false))
                    {
                        return await ReadResponseJson<string>(message);
                    }
                }
            }
        }
    }
}
