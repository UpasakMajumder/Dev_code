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
        private const string _serviceUrlSettingKey = "KDA_NotificationServiceUrl";
        private readonly IMicroProperties _properties;

        public NotificationClient(IMicroProperties properties)
        {
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public async Task<BaseResponseDto<object>> SendCustomNotification(SendCustomNotificationRequestDto request)
        {
            var url = _properties.GetServiceUrl(_serviceUrlSettingKey);
            url = $"{url}/api/notification/sendmails";
            return await Post<object>(url, request).ConfigureAwait(false);
        }
    }
}

