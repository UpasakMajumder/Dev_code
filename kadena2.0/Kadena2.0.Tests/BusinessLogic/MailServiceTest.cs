using Kadena.BusinessLogic.Services;
using Kadena.Dto.General;
using Kadena.Dto.Notification.MicroserviceRequests;
using Kadena.Models.TemplatedProduct;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class MailServiceTest
    {

        [Fact]
        public async Task MailService_BasicTest()
        {
            // Arrange
            const string subject = "subj";
            const string html = "<p>message</p><a href=\"url\">url</a>";

            var notificationMock = new Mock<INotificationClient>();
            notificationMock.Setup(m => m.SendCustomNotification( 
                It.Is<SendCustomNotificationRequestDto>(o => o.Email == "mail1@d.cz;mail2@d.cz" && o.Subject == subject && o.Html == html)))
                .Returns(Task.FromResult( new BaseResponseDto<object>{ Success = true }));

            var request = new EmailProofRequest
            {
                EmailProofUrl = "url",
                Message = "message",
                Subject = "subj",
                RecepientEmail = "mail1@d.cz, mail2@d.cz"
            };

            var sut = new MailService(notificationMock.Object, Mock.Of<IKenticoLogger>());

            // Act 
            await sut.SendProofMail(request);

            // Assert
            notificationMock.Verify(m => m.SendCustomNotification(
                It.Is<SendCustomNotificationRequestDto>(o => o.Email == "mail1@d.cz;mail2@d.cz" && o.Subject == subject && o.Html == html)),Times.Once);
        }
    }
}
