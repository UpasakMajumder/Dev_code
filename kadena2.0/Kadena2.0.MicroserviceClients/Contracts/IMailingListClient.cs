﻿using Kadena.Dto.General;
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
        Task<BaseResponseDto<MailingListDataDTO[]>> GetMailingListsForCustomer();

        /// <summary>
        /// Removes specified mailing list.
        /// </summary>
        Task<BaseResponseDto<object>> RemoveMailingList(Guid mailingListId);

        /// <summary>
        /// Removes mailing lists with validity date older than speficied date.
        /// </summary>
        Task<BaseResponseDto<object>> RemoveMailingList(DateTime olderThan);

        /// <summary>
        /// Removes all address from specified container.
        /// </summary>
        /// <param name="containerId">Id of container to be cleared.</param>
        /// <param name="addressIds">If specified removes only addresses from list.</param>
        Task<BaseResponseDto<object>> RemoveAddresses(Guid containerId, IEnumerable<Guid> addressIds = null);
        
        /// <summary>
        /// Gets list of addresses in specified container.
        /// </summary>
        /// <param name="containerId">Id of container.</param>
        /// <returns>List of addresses.</returns>
        Task<BaseResponseDto<IEnumerable<MailingAddressDto>>> GetAddresses(Guid containerId);

        Task<BaseResponseDto<IEnumerable<string>>> UpdateAddresses(Guid containerId, IEnumerable<MailingAddressDto> addresses);

        /// <summary>
        /// Get all mailing list for particular customer (whole site) by specified Id.
        /// </summary>
        /// <param name="containerId">Id of container to get.</param>
        Task<BaseResponseDto<MailingListDataDTO>> GetMailingList(Guid containerId);

        /// <summary>
        /// Sends request to microservice to create mailing container.
        /// </summary>
        /// <param name="name">Name for mailing container.</param>
        /// <param name="mailType">Mail type option for mailing container.</param>
        /// <param name="product">Product type option for mailing container.</param>
        /// <param name="validityDays">Validity option for mailing container.</param>
        /// <returns>Id of mailing container.</returns>
        Task<BaseResponseDto<Guid>> CreateMailingContainer(string name, string mailType, string product, int validityDays, string customerId);

        /// <summary>
        /// Uploads specified mapping to microservice with binding to specified file and container.
        /// </summary>
        /// <param name="fileId">Id of file.</param>
        /// <param name="containerId">Id of mailing container.</param>
        /// <param name="mapping">Dictionary with mapping field names to index of column.</param>
        Task<BaseResponseDto<object>> UploadMapping(string fileId, Guid containerId, Dictionary<string, int> mapping);
    }
}
