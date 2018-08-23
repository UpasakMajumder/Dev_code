using System;
using Kadena.BusinessLogic.Contracts;
using Kadena.Dto.Notification.MicroserviceRequests;
using Kadena.Models.TemplatedProduct;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services
{
    public class MailService : IMailService
    {
        private readonly INotificationClient _notificationClient;
        private readonly IKenticoLogger _log;

        public MailService(INotificationClient notificationClient, IKenticoLogger log)
        {
            _notificationClient = notificationClient ?? throw new ArgumentNullException(nameof(notificationClient));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public async Task SendProofMail(EmailProofRequest request)
        {
            var mails = request.RecepientEmail.Replace(',', ';').Replace(" ", string.Empty);
            var html = $"<p>{request.Message}</p><a href=\"{request.EmailProofUrl}\">{request.EmailProofUrl}</a>";

            var microserviceRequest = new SendCustomNotificationRequestDto
            {
                Email = mails,
                Subject = request.Subject,
                Html = html
            };

            var sendResult = await _notificationClient.SendCustomNotification(microserviceRequest).ConfigureAwait(false);

            if (!sendResult.Success)
            {
                _log.LogError("SendEmailProof", $"Calling Notification microservice failed. {sendResult.ErrorMessages}");
            }

        }
    }
}
