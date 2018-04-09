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
        private readonly INotificationClient notificationClient;
        private readonly IKenticoLogger log;

        public MailService(INotificationClient notificationClient)
        {
            this.notificationClient = notificationClient ?? throw new System.ArgumentNullException(nameof(notificationClient));
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

            var sendResult = await notificationClient.SendCustomNotification(microserviceRequest);

            if (!sendResult.Success)
            {
                log.LogError("SendEmailProof", $"Calling Notification microservice failed. {sendResult.ErrorMessages}");
            }

        }
    }
}
