using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Clients.Base;
using System.Threading.Tasks;
using System;
using Kadena2.MicroserviceClients.Contracts.Base;
using Kadena.Dto.Notification.MicroserviceRequests;
using Kadena2.MicroserviceClients.Contracts;

namespace Kadena2.MicroserviceClients.Clients
{
    public sealed class NotificationClient : SignedClientBase, INotificationClient
    {
        public NotificationClient(IMicroProperties properties)
        {
            _serviceUrlSettingKey = "KDA_NotificationServiceUrl";
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public async Task<BaseResponseDto<object>> SendCustomNotification(SendCustomNotificationRequestDto request)
        {
            var url = $"{BaseUrlOld}/api/notification/sendmails";
            return await Post<object>(url, request).ConfigureAwait(false);
        }
    }
}

