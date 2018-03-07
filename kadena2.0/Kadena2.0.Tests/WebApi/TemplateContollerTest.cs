using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.Controllers;
using Moq;
using Moq.AutoMock;
using System;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Xunit;

namespace Kadena.Tests.WebApi
{
    public class TemplateContollerTest
    {
        [Fact(DisplayName = "TemplateController.GetPreview() | NotFound")]
        public async Task GetPreviewNotFound()
        {
            var autoMock = new AutoMocker();
            var sut = autoMock.CreateInstance<TemplateController>();

            var actualResult = await sut.GetPreview(Guid.Empty, Guid.Empty);

            Assert.IsType<NotFoundResult>(actualResult);
            Assert.NotNull(actualResult);
        }

        [Fact(DisplayName = "TemplateController.GetPreview() | Redirect")]
        public async Task GetPreviewRedirect()
        {
            var expectedResult = new Uri("http://example.com");
            var autoMock = new AutoMocker();
            var templateService = autoMock.GetMock<ITemplateService>();
            templateService
                .Setup(cl => cl.GetPreviewUri(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(expectedResult));
            var sut = autoMock.CreateInstance<TemplateController>();

            var actualResult = await sut.GetPreview(Guid.Empty, Guid.Empty);

            Assert.IsType<RedirectResult>(actualResult);
            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, (actualResult as RedirectResult).Location);
        }
    }
}
