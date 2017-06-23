using Kadena.Dto.General;
using Kadena.Dto.MailingList.MicroserviceResponses;
using Kadena2.MicroserviceClients.Clients.Base;
using Kadena2.MicroserviceClients.Contracts;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Kadena2.MicroserviceClients.Clients
{
    public class MailingListClient : ClientBase, IMailingListClient
    {
        public async Task<BaseResponse<MailingListDataDTO[]>> GetMailingListsForCustomer(string serviceEndpoint, string customerName)
        {
            using (var httpClient = new HttpClient())
            {
                var encodedCustomerName = HttpUtility.UrlEncode(customerName);
                // TODO remove redundant settings keys
                // var url = $"{serviceEndpoint.TrimEnd('/')}/api/mailing/allforcustomer/{encodedCustomerName}";
                var url = $"{serviceEndpoint.TrimEnd('/')}/{encodedCustomerName}";
                using (var response = await httpClient.GetAsync(url))
                {
                    return await ReadResponseJson<MailingListDataDTO[]>(response);
                }
            }
        }
    }
}

