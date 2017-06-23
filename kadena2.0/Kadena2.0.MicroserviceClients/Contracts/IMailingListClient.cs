using Kadena.Dto.General;
using Kadena.Dto.MailingList.MicroserviceResponses;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface IMailingListClient
    {
        Task<BaseResponseDTO<MailingListDataDTO[]>> GetMailingListsForCustomer(string serviceEndpoint, string customerName);
    }
}
