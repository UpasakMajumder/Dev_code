using Kadena.Dto.General;
using Kadena.Dto.Notification.MicroserviceRequests;
using System.Threading.Tasks;

namespace Kadena2.MicroserviceClients.Contracts
{
    public interface INotificationClient
    {
        Task<BaseResponseDto<object>> SendCustomNotification(SendCustomNotificationRequestDto request);
    }
}
