using Xunit;
using Kadena.Dto.Approval.MicroserviceRequests;

namespace Kadena.Tests.Dto.Approval.MicroserviceRequests
{
    public class ApprovalStateTests
    {
        [Fact(DisplayName = "ApprovalState.Approved")]
        public void Approved()
        {
            var expectedResult = 200;

            var actualResult = (int)ApprovalState.Approved;

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "ApprovalState.Rejected")]
        public void Rejected()
        {
            var expectedResult = 300;

            var actualResult = (int)ApprovalState.Rejected;

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
