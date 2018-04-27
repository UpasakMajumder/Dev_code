using Kadena.BusinessLogic.Services.Approval;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Moq;
using Xunit;

namespace Kadena.Tests.BusinessLogic.Approval
{
    public class ApproverServiceTests : KadenaUnitTest<ApproverService>
    {
        [Fact]
        public void GetApproversTest()
        {
            //Sut.GetApprovers(1);

            //Verify<IKenticoRoleProvider>(p => p.GetRoleUsers(Sut.ApproversRoleName, 1), Times.Once);
        }
    }
}
