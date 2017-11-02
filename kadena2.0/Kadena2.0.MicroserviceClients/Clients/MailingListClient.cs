using Kadena.Dto.General;
using Kadena.Dto.MailingList.MicroserviceResponses;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Kadena.KOrder.PaymentService.Infrastucture.Helpers;
using System.IO;

namespace Kadena2.MicroserviceClients.Clients
{
    public class MailingListClient : ClientBase, IMailingListClient
    {
        //public MailingListClient() : base()
        //{

        //}

        //public MailingListClient(IAwsV4Signer signer) : base(signer)
        //{

        //}

        public async Task<BaseResponseDto<IEnumerable<MailingAddressDto>>> GetAddresses(string serviceEndpoint, Guid containerId)
        {
            var url = $"{serviceEndpoint}/api/DeliveryAddress/ByContainer/{containerId}";
            return await Get<IEnumerable<MailingAddressDto>>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<MailingListDataDTO>> GetMailingList(string serviceEndpoint, string customerName, Guid containerId)
        {
            string url = $"{serviceEndpoint}/api/Mailing/ByCustomerAndId/{customerName}/{containerId}";
            return await Get<MailingListDataDTO>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<MailingListDataDTO[]>> GetMailingListsForCustomer(string serviceEndpoint, string customerName)
        {
            var encodedCustomerName = HttpUtility.UrlEncode(customerName);
            var url = $"{serviceEndpoint.TrimEnd('/')}/api/Mailing/AllForCustomer/{encodedCustomerName}";
            return await Get<MailingListDataDTO[]>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<object>> RemoveMailingList(string serviceEndpoint, string customerName, Guid mailingListId)
        {
            var encodedCustomerName = HttpUtility.UrlEncode(customerName);
            var url = $"{serviceEndpoint.TrimEnd('/')}/api/Mailing/ByCustomerAndId/{encodedCustomerName}/{mailingListId}";
            return await Delete<object>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<object>> RemoveMailingList(string serviceEndpoint, string customerName, DateTime olderThan)
        {
            var url = $"{serviceEndpoint}/api/Mailing/ByFilter";
            var body = new
            {
                customerName = customerName,
                validTo = olderThan
            };

            return await Delete<object>(url, body).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<object>> RemoveAddresses(string serviceEndpoint, string customerName, Guid containerId, IEnumerable<Guid> addressIds = null)
        {
            var url = $"{serviceEndpoint}/api/DeliveryAddress/BulkDelete";
            var body = new
            {
                ContainerId = containerId,
                ids = addressIds,
                CustomerName = customerName
            };

            return await Delete<object>(url, body).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<IEnumerable<string>>> UpdateAddresses(string serviceEndpoint, string customerName, Guid containerId, IEnumerable<MailingAddressDto> addresses)
        {
            var url = $"{serviceEndpoint}/api/DeliveryAddress/ManualBulkUpdate";
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("PATCH"), url)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        CustomerName = customerName,
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
                    }), System.Text.Encoding.UTF8, "application/json"),
                })
                {
                    using (var message = await client.SendAsync(request).ConfigureAwait(false))
                    {
                        return await ReadResponseJson<IEnumerable<string>>(message).ConfigureAwait(false);
                    }
                }
            }
        }

        public async Task<BaseResponseDto<string>> Validate(string serviceEndpoint, string customerName, Guid containerId)
        {
            var url = $"{serviceEndpoint}";
            var body = new
            {
                ContainerId = containerId,
                CustomerName = customerName
            };

            return await Post<string>(url, body).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<Guid>> CreateMailingContainer(string endPoint, string customerName, string name, string mailType, string product, int validityDays, string customerId)
        {
            var createContainerUrl = $"{endPoint}/api/Mailing";
            return await Post<Guid>(createContainerUrl, new
            {
                name = name,
                customerName = customerName,
                Validity = validityDays,
                mailType = mailType,
                productType = product,
                customerId = customerId
            }).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<object>> UploadMapping(string endPoint, string customerName, string fileId, Guid containerId, Dictionary<string, int> mapping)
        {
            var uploadMappingUrl = $"{endPoint}/api/DeliveryAddress";
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
            return await Post<object>(uploadMappingUrl, new
            {
                mapping = jsonMapping,
                fileId = fileId,
                customerName = customerName,
                containerId = containerId
            }).ConfigureAwait(false);
        }
    }
}

