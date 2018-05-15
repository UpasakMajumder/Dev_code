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
        public static IEnumerable<object[]> GetDependencies()
        {
            yield return new object[]
            {
                null,
                new Mock<IMapper>().Object,
            };
            yield return new object[]
            {
                new Mock<IKListService>().Object,
                null,
            };
        }

        [Theory(DisplayName = "KListController()")]
        [MemberData(nameof(GetDependencies))]
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
    }
}
