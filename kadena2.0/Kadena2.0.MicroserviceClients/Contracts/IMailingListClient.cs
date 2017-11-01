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
        /// Removes specified mailing list.
        /// </summary>
        Task<BaseResponseDto<object>> RemoveMailingList(string serviceEndpoint, string customerName, Guid mailingListId);

        /// <summary>
        /// Removes mailing lists with validity date older than speficied date.
        /// </summary>
        Task<BaseResponseDto<object>> RemoveMailingList(string serviceEndpoint, string customerName, DateTime olderThan);

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

        /// <summary>
        /// Gets list of addresses in specified container.
        /// </summary>
        /// <param name="containerId">Id of container.</param>
        /// <returns>List of addresses.</returns>
        Task<BaseResponseDto<IEnumerable<MailingAddressDto>>> GetAddresses(string serviceEndpoint, Guid containerId);

        Task<BaseResponseDto<IEnumerable<string>>> UpdateAddresses(string serviceEndpoint, string customerName, Guid containerId, IEnumerable<MailingAddressDto> addresses);

        /// <summary>
        /// Get all mailing list for particular customer (whole site) by specified Id.
        /// </summary>
        /// <param name="containerId">Id of container to get.</param>
        Task<BaseResponseDto<MailingListDataDTO>> GetMailingList(string serviceEndpoint, string customerName, Guid containerId);

        /// <summary>
        /// Sends request to microservice to create mailing container.
        /// </summary>
        /// <param name="name">Name for mailing container.</param>
        /// <param name="mailType">Mail type option for mailing container.</param>
        /// <param name="product">Product type option for mailing container.</param>
        /// <param name="validityDays">Validity option for mailing container.</param>
        /// <returns>Id of mailing container.</returns>
        Task<BaseResponseDto<Guid>> CreateMailingContainer(string endPoint, string customerName, string name, string mailType, string product, int validityDays, string customerId);
    }
}
