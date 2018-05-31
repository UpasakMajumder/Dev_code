using Xunit;
using Kadena.Dto.Approval.MicroserviceRequests;
using Kadena.Helpers;

namespace Kadena.Tests.Dto.Approval.MicroserviceRequests
{
    public class ApprovalStateTests
    {
        [Fact(DisplayName = "ApprovalState.Approved | Integer value")]
        public void Approved_Integer()
        {
            var expectedResult = 200;

            var actualResult = (int)ApprovalState.Approved;

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "ApprovalState.Approved | String value")]
        public void Approved_String()
        {
            var expectedResult = "Approved";

            var actualResult = ApprovalState.Approved.ToString();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "ApprovalState.Approved | Display attribute")]
        public void Approved_Display()
        {
            var expectedResult = "ApproveOrder";

            var actualResult = ApprovalState.Approved.GetDisplayName();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "ApprovalState.Rejected | Integer value")]
        public void Rejected_Integer()
        {
            var expectedResult = 300;

            var actualResult = (int)ApprovalState.ApprovalRejected;

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "ApprovalState.Rejected | String value")]
        public void Rejected_String()
        {
            var expectedResult = "ApprovalRejected";

            var actualResult = ApprovalState.ApprovalRejected.ToString();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact(DisplayName = "ApprovalState.Rejected | Display attribute")]
        public void Rejected_Display()
        {
            var expectedResult = "RejectOrder";

            var actualResult = ApprovalState.ApprovalRejected.GetDisplayName();

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
