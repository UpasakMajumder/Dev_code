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
            var url = $"{serviceEndpoint}/{containerId}";
            return await Get<IEnumerable<MailingAddressDto>>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<MailingListDataDTO>> GetMailingList(string serviceEndpoint, string customerName, Guid containerId)
        {
            string url = $"{serviceEndpoint}/{customerName}/{containerId}";
            return await Get<MailingListDataDTO>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<MailingListDataDTO[]>> GetMailingListsForCustomer(string serviceEndpoint, string customerName)
        {
            var encodedCustomerName = HttpUtility.UrlEncode(customerName);
            var url = $"{serviceEndpoint.TrimEnd('/')}/{encodedCustomerName}";
            return await Get<MailingListDataDTO[]>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<object>> RemoveMailingList(string serviceEndpoint, string customerName, Guid mailingListId)
        {
            var encodedCustomerName = HttpUtility.UrlEncode(customerName);
            var url = $"{serviceEndpoint.TrimEnd('/')}/{encodedCustomerName}/{mailingListId}";
            return await Delete<object>(url).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<object>> RemoveMailingList(string serviceEndpoint, string customerName, DateTime olderThan)
        {
            var body = new
            {
                customerName = customerName,
                validTo = olderThan
            };

            return await Delete<object>(serviceEndpoint, body).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<object>> RemoveAddresses(string serviceEndpoint, string customerName, Guid containerId, IEnumerable<Guid> addressIds = null)
        {
            var body = new
            {
                ContainerId = containerId,
                ids = addressIds,
                CustomerName = customerName
            };

            return await Delete<object>(serviceEndpoint, body).ConfigureAwait(false);
        }

        public async Task<BaseResponseDto<IEnumerable<string>>> UpdateAddresses(string serviceEndpoint, string customerName, Guid containerId, IEnumerable<MailingAddressDto> addresses)
        {
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("PATCH"), serviceEndpoint)
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
            var body = new
            {
                ContainerId = containerId,
                CustomerName = customerName
            };

            return await Post<string>(serviceEndpoint, body).ConfigureAwait(false);
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
    }
}

