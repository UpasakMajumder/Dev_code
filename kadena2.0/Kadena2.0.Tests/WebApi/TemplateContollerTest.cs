using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.Controllers;
using Moq;
using System;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Xunit;

namespace Kadena.Tests.WebApi
{
    public class TemplateContollerTest : KadenaUnitTest<TemplateController>
    {
        [Fact(DisplayName = "TemplateController.GetPreview() | NotFound")]
        public async Task GetPreviewNotFound()
        {
            var actualResult = await Sut.GetPreview(Guid.Empty, Guid.Empty);

            Assert.IsType<NotFoundResult>(actualResult);
            Assert.NotNull(actualResult);
        }

        [Fact(DisplayName = "TemplateController.GetPreview() | Redirect")]
        public async Task GetPreviewRedirect()
        {
            var expectedResult = new Uri("http://example.com");
            Setup<ITemplateService, Task<Uri>>(cl => cl.GetPreviewUri(It.IsAny<Guid>(), It.IsAny<Guid>()), Task.FromResult(expectedResult));

            var actualResult = await Sut.GetPreview(Guid.Empty, Guid.Empty);

            Assert.IsType<RedirectResult>(actualResult);
            Assert.NotNull(actualResult);
            Assert.Equal(expectedResult, (actualResult as RedirectResult).Location);
        }
    }
}
