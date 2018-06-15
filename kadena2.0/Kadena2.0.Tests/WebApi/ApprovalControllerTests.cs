using Kadena.WebAPI.Controllers;
using System;
using Xunit;
using System.Threading.Tasks;
using Kadena.BusinessLogic.Contracts.Approval;
using System.Web.Http.Results;
using Kadena.WebAPI.Infrastructure.Communication;
using Kadena.Dto.Approval.Responses;
using Kadena.Dto.Approval.Requests;
using AutoMapper;

namespace Kadena.Tests.WebApi
{
    public class ApprovalControllerTests : KadenaUnitTest<ApprovalController>
    {
        [Theory(DisplayName = "ApprovalController()")]
        [ClassData(typeof(ApprovalControllerTests))]
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
