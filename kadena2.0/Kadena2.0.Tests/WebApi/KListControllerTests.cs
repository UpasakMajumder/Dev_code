using Xunit;
using Moq;
using Kadena.WebAPI.Controllers;
using System.Collections.Generic;
using System;
using AutoMapper;
using Kadena.BusinessLogic.Contracts;
using System.Web.Http.Results;
using Kadena.WebAPI.Infrastructure.Communication;
using System.Threading.Tasks;
using Kadena.Models;

namespace Kadena.Tests.WebApi
{
    public class KListControllerTests : KadenaUnitTest<KListController>
    {
        [Theory(DisplayName = "KListController()")]
        [ClassData(typeof(KListControllerTests))]
        public void KListController(IKListService kListService, IMapper mapper)
        {
            Assert.Throws<ArgumentNullException>(() => new KListController(kListService, mapper));
        }

        [Fact(DisplayName = "KListController.UseOnlyCorrect() | Success")]
        public async Task UseOnlyCorrect_Success()
        {
            var containerId = Guid.NewGuid();

            Setup<IKListService, Task<bool>>(s => s.UseOnlyCorrectAddresses(containerId), Task.FromResult(true));

            var actualResult = await Sut.UseOnlyCorrect(containerId);

            Assert.IsType<JsonResult<BaseResponse<bool>>>(actualResult);
        }

        [Fact(DisplayName = "KListController.UseOnlyCorrect() | Failed")]
        public async Task UseOnlyCorrect_Failed()
        {
            var containerId = Guid.NewGuid();

            Setup<IKListService, Task<bool>>(s => s.UseOnlyCorrectAddresses(containerId), Task.FromResult(false));

            var actualResult = await Sut.UseOnlyCorrect(containerId);

            Assert.IsType<JsonResult<ErrorResponse>>(actualResult);
        }

        [Fact(DisplayName = "KListController.Update() | Success")]
        public async Task Update_Success()
        {
            var containerId = Guid.NewGuid();

            Setup<IKListService, Task<bool>>(s => s.UpdateAddresses(containerId, It.IsAny<MailingAddress[]>()), Task.FromResult(true));

            var actualResult = await Sut.Update(containerId, null);

            Assert.IsType<JsonResult<BaseResponse<bool>>>(actualResult);
        }

        [Fact(DisplayName = "KListController.Update() | Failed")]
        public async Task Update_Failed()
        {
            var containerId = Guid.NewGuid();

            Setup<IKListService, Task<bool>>(s => s.UpdateAddresses(containerId, It.IsAny<MailingAddress[]>()), Task.FromResult(false));

            var actualResult = await Sut.Update(containerId, null);

            Assert.IsType<JsonResult<ErrorResponse>>(actualResult);
        }

        [Fact(DisplayName = "KListController.Export() | Success")]
        public async Task Export_Success()
        {
            var containerId = Guid.NewGuid();
            Setup<IKListService, Task<Uri>>(s => s.GetContainerFileUrl(containerId), Task.FromResult(new Uri("https://google.com")));

            var actualResult = await Sut.Export(containerId);

            Assert.IsType<RedirectResult>(actualResult);
        }

        [Fact(DisplayName = "KListController.Export() | Failed")]
        public async Task Export_Failed()
        {
            var containerId = Guid.NewGuid();

            var actualResult = await Sut.Export(containerId);

            Assert.IsType<NotFoundResult>(actualResult);
        }
    }
}
