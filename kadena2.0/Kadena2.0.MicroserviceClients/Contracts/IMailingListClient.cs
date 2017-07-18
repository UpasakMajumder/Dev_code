using Kadena.Dto.General;
using Kadena.Dto.MailingList.MicroserviceResponses;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IMailingListClient
    {
        /// <summary>
        /// Get all mailing lists for particular customer (whole site)
        /// </summary>
        Task<BaseResponseDto<MailingListDataDTO[]>> GetMailingListsForCustomer(string serviceEndpoint, string customerName);
    }
}
