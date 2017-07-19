using Kadena.Dto.General;
using Kadena.Dto.MailingList.MicroserviceResponses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IMailingListClient
    {
        /// <summary>
        /// Get all mailing lists for particular customer (whole site)
        /// </summary>
        Task<BaseResponseDto<MailingListDataDTO[]>> GetMailingListsForCustomer(string serviceEndpoint, string customerName);

        Task<BaseResponseDto<object>> RemoveAddresses(string serviceEndpoint, string customerName, Guid containerId, IEnumerable<Guid> addressIds = null);

        Task<BaseResponseDto<string>> Validate(string serviceEndpoint, string customerName, Guid containerId);
    }
}
