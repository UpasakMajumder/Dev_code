using Kadena.BusinessLogic.Services.Approval;
using Kadena2.WebAPI.KenticoProviders.Contracts;
using Moq;
using Xunit;

namespace Kadena.Tests.BusinessLogic.Approval
{
    public class ApproverServiceTests : KadenaUnitTest<ApproverService>
    {
        [Fact]
        public void GetApproversTest()
        {
            const int siteId = 1;

            Sut.GetApprovers(siteId);

            Verify<IKenticoPermissionsProvider>(p => p.GetUsersWithApproverPermission(siteId), Times.Once);
        }
    }
}
