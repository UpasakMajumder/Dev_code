using Kadena.Dto.General;
using Kadena.Dto.MailingList.MicroserviceResponses;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IMailingListClient
    {
        Task<BaseResponseDto<MailingListDataDTO[]>> GetMailingListsForCustomer(string serviceEndpoint, string customerName);
    }
}
