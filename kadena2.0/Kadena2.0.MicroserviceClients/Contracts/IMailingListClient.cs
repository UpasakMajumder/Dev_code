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

        /// <summary>
        /// Removes all address from specified container.
        /// </summary>
        /// <param name="containerId">Id of container to be cleared.</param>
        /// <param name="addressIds">If specified removes only addresses from list.</param>
        Task<BaseResponseDto<object>> RemoveAddresses(string serviceEndpoint, string customerName, Guid containerId, IEnumerable<Guid> addressIds = null);

        /// <summary>
        /// Forces microservices to start addresses validation for specified container.
        /// </summary>
        /// <param name="containerId">Id of container.</param>
        /// <returns>Id of file with valid addresses.</returns>
        Task<BaseResponseDto<string>> Validate(string serviceEndpoint, string customerName, Guid containerId);
    }
}
