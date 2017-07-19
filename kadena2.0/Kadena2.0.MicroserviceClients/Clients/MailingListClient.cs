using Kadena.Dto.General;
using Kadena.Dto.MailingList.MicroserviceResponses;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Kadena2.MicroserviceClients.Clients
{
    public class MailingListClient : ClientBase, IMailingListClient
    {
        public async Task<BaseResponseDto<IEnumerable<MailingAddressDto>>> GetAddresses(string serviceEndpoint, Guid containerId)
        {
            using (var client = new HttpClient())
            {
                using (var message = await client.GetAsync($"{serviceEndpoint}/{containerId}"))
                {
                    return await ReadResponseJson<IEnumerable<MailingAddressDto>>(message);
                }
            }
        }

        public async Task<BaseResponseDto<MailingListDataDTO[]>> GetMailingListsForCustomer(string serviceEndpoint, string customerName)
        {
            using (var httpClient = new HttpClient())
            {
                var encodedCustomerName = HttpUtility.UrlEncode(customerName);
                // TODO remove redundant settings keys
                // var url = $"{serviceEndpoint.TrimEnd('/')}/api/mailing/allforcustomer/{encodedCustomerName}";
                var url = $"{serviceEndpoint.TrimEnd('/')}/{encodedCustomerName}";
                using (var response = await httpClient.GetAsync(url).ConfigureAwait(false))
                {
                    return await ReadResponseJson<MailingListDataDTO[]>(response);
                }
            }
        }

        public async Task<BaseResponseDto<object>> RemoveAddresses(string serviceEndpoint, string customerName, Guid containerId, IEnumerable<Guid> addressIds = null)
        {
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        ContainerId = containerId,
                        ids = addressIds,
                        CustomerName = customerName
                    }), System.Text.Encoding.UTF8, "application/json"),
                    RequestUri = new Uri(serviceEndpoint),
                    Method = HttpMethod.Delete
                })
                {
                    using (var message = await client.SendAsync(request).ConfigureAwait(false))
                    {
                        return await ReadResponseJson<object>(message);
                    }
                }
            }
        }

        public async Task<BaseResponseDto<string>> Validate(string serviceEndpoint, string customerName, Guid containerId)
        {
            using (var client = new HttpClient())
            {
                using (var content = new StringContent(JsonConvert.SerializeObject(new
                {
                    ContainerId = containerId,
                    CustomerName = customerName
                }), System.Text.Encoding.UTF8, "application/json"))
                {
                    using (var message = await client.PostAsync(serviceEndpoint, content).ConfigureAwait(false))
                    {
                        return await ReadResponseJson<string>(message);
                    }
                }
            }
        }
    }
}

