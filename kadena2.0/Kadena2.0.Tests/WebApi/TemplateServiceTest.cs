using Kadena.Dto.General;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.WebAPI.Services;
using Kadena2.MicroserviceClients.Contracts;
using Moq;
using Xunit;
using System.Threading.Tasks;
using System;

namespace Kadena.Tests.WebApi
{
    public class TemplateServiceTest
    {
        private string _currentName = "currentName";

        private TemplateService Create(Mock<ITemplatedProductService> templateClient = null)
        {
            var kenticoLogger = new Mock<IKenticoLogger>();
            var kenticoClient = new Mock<IKenticoResourceService>();

            return new TemplateService(kenticoClient.Object, kenticoLogger.Object,
                templateClient?.Object ?? new Mock<ITemplatedProductService>().Object);
        }

        private BaseResponseDto<bool?> SetNameSuccess(string name)
        {
            _currentName = name;
            return new BaseResponseDto<bool?>
            {
                Success = true,
                Payload = true
            };
        }

        private BaseResponseDto<bool?> SetNameFailed()
        {
            return new BaseResponseDto<bool?>
            {
                Success = false,
                Payload = null,
                ErrorMessages = "Some error."
            };
        }

        [Fact(DisplayName = "SetNameSucceed")]
        public async Task SetNameSucceed()
        {
            var newName = "newName";
            Assert.NotEqual(newName, _currentName);

            var client = new Mock<ITemplatedProductService>();
            client.Setup(c => c.SetName(null, Guid.Empty, newName))
                .Returns(Task.FromResult(SetNameSuccess(newName)));
            var service = Create(client);

            var result = await service.SetName(Guid.Empty, newName);
            Assert.True(result);
            Assert.Equal(newName, _currentName);
        }

        [Fact(DisplayName = "SetNameFail")]
        public async Task SetNameFail()
        {
            var newName = "newName";

            var client = new Mock<ITemplatedProductService>();
            client.Setup(c => c.SetName(null, Guid.Empty, newName))
                .Returns(Task.FromResult(SetNameFailed()));
            var service = Create(client);
            var result = await service.SetName(Guid.Empty, newName);

            Assert.False(result);
        }
    }
}
