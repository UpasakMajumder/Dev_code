using Kadena.WebAPI.Controllers;
using System;
using Xunit;
using Moq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Kadena.BusinessLogic.Contracts.Approval;
using System.Web.Http.Results;
using Kadena.WebAPI.Infrastructure.Communication;
using Kadena.Dto.Approval.Responses;
using Kadena.Dto.Approval.Requests;
using AutoMapper;
using Kadena.Container.Default;
using Kadena.Models.Approval;

namespace Kadena.Tests.WebApi
{
    public class ApprovalControllerTests : KadenaUnitTest<ApprovalController>
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
                    new Mock<IApprovalService>().Object,
                    null,
                };
        }

        [Theory(DisplayName = "ApprovalController()")]
        [MemberData(nameof(GetDependencies))]
        public void ApprovalController(IApprovalService approvalService, IMapper mapper)
        {
            Assert.Throws<ArgumentNullException>(() => new ApprovalController(approvalService, mapper));
        }

        [Fact(DisplayName = "ApprovalController.Approve()")]
        public async Task Approve()
        {
            var actualResult = await Sut.Approve(new ApprovalRequestDto());

            Assert.IsType<JsonResult<BaseResponse<ApprovalResultDto>>>(actualResult);
        }

        [Fact(DisplayName = "ApprovalController.Reject()")]
        public async Task Reject()
        {
            var actualResult = await Sut.Reject(new ApprovalRequestDto());

            Assert.IsType<JsonResult<BaseResponse<ApprovalResultDto>>>(actualResult);
        }
    }
}
