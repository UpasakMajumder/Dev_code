using Kadena.Dto.General;
using Kadena.Dto.MailingList.MicroserviceResponses;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;
using System.Web;
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Kadena2.MicroserviceClients.Contracts.Base;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class MailingListClient : SignedClientBase, IMailingListClient
    {
        public MailingListClient(IMicroProperties properties)
        {
            _serviceUrlSettingKey = "KDA_MailingServiceUrl";
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public async Task<BaseResponseDto<IEnumerable<MailingAddressDto>>> GetAddresses(Guid containerId)
        {
            var url = $"{BaseUrl}/api/DeliveryAddress/ByContainer/{containerId}";
            return await Get<IEnumerable<MailingAddressDto>>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<MailingListDataDTO>> GetMailingList(Guid containerId)
        {
            var encodedCustomerName = HttpUtility.UrlEncode(_properties.GetCustomerName());
            var url = $"{BaseUrl}/api/Mailing/ByCustomerAndId/{encodedCustomerName}/{containerId}";
            return await Get<MailingListDataDTO>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<MailingListDataDTO[]>> GetMailingListsForCustomer()
        {
            var encodedCustomerName = HttpUtility.UrlEncode(_properties.GetCustomerName());
            var url = $"{BaseUrl}/api/Mailing/AllForCustomer/{encodedCustomerName}";
            return await Get<MailingListDataDTO[]>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<object>> RemoveMailingList(Guid mailingListId)
        {
            var encodedCustomerName = HttpUtility.UrlEncode(_properties.GetCustomerName());
            var url = $"{BaseUrl}/api/Mailing/ByCustomerAndId/{encodedCustomerName}/{mailingListId}";
            return await Delete<object>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<object>> RemoveMailingList(DateTime olderThan)
        {
            var url = $"{BaseUrl}/api/Mailing/ByFilter";
            var body = new
            {
                customerName = _properties.GetCustomerName(),
                validTo = olderThan
            };

            return await Delete<object>(url, body).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<object>> RemoveAddresses(Guid containerId, IEnumerable<Guid> addressIds = null)
        {
            var url = $"{BaseUrl}/api/DeliveryAddress/BulkDelete";
            var body = new
            {
                ContainerId = containerId,
                ids = addressIds,
                CustomerName = _properties.GetCustomerName()
            };

            return await Delete<object>(url, body).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<IEnumerable<string>>> UpdateAddresses(Guid containerId, IEnumerable<MailingAddressDto> addresses)
        {
            var url = $"{BaseUrl}/api/DeliveryAddress/ManualBulkUpdate";
            var requestBody = new
            {
                CustomerName = _properties.GetCustomerName(),
                UpdateObjects = addresses.Select(a => new
                {
                    HashKey = containerId,
                    RangeKey = a.Id,
                    UpdateField = new Dictionary<string, string> {
                                { nameof(a.FirstName), a.FirstName ?? string.Empty },
                                { nameof(a.Address1), a.Address1 ?? string.Empty },
                                { nameof(a.Address2), a.Address2 ?? string.Empty },
                                { nameof(a.City), a.City ?? string.Empty },
                                { nameof(a.State), a.State ?? string.Empty },
                                { nameof(a.Zip), a.Zip ?? string.Empty },
                                { nameof(a.ErrorMessage), string.Empty }
                            }
                })
            };
            return await Patch<IEnumerable<string>>(url, requestBody).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<Guid>> CreateMailingContainer(string name, string mailType, string product, int validityDays, string customerId)
        {
            var createContainerUrl = $"{BaseUrl}/api/Mailing";
            return await Post<Guid>(createContainerUrl, new
            {
                name = name,
                customerName = _properties.GetCustomerName(),
                Validity = validityDays,
                mailType = mailType,
                productType = product,
                customerId = customerId
            }).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<object>> UploadMapping(string fileId, Guid containerId, Dictionary<string, int> mapping)
        {
            var uploadMappingUrl = $"{BaseUrl}/api/DeliveryAddress";
            string jsonMapping = string.Empty;
            using (var sw = new StringWriter())
            {
                using (var writer = new JsonTextWriter(sw))
                {
                    writer.WriteStartObject();
                    foreach (var map in mapping)
                    {
                        writer.WritePropertyName(map.Key);
                        writer.WriteValue(map.Value);
                    }
                    writer.WriteEndObject();
                    jsonMapping = sw.ToString();
                }
            }
            var requestBody = new
            {
                mapping = jsonMapping,
                fileId = fileId,
                customerName = _properties.GetCustomerName(),
                containerId = containerId
            };
            return await Post<object>(uploadMappingUrl, requestBody).ConfigureAwait(false);
        }
    }
}

