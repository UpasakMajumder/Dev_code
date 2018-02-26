using Kadena.Dto.General;
using Kadena.MicroserviceClients.Clients;
using Moq.AutoMock;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.MicroserviceClients
{
    public class TemplateClientTest
    {
        [Fact(DisplayName = "TemplateClient.GetPreview()")]
        public async Task GetPreview()
        {
            var autoMock = new AutoMocker();
            var sut = autoMock.CreateInstance<TemplatedClient>();

            var actualResult = await sut.GetPreview(Guid.Empty, Guid.Empty);

            Assert.IsType<BaseResponseDto<string>>(actualResult);
            Assert.NotNull(actualResult);
        }
    }
}
