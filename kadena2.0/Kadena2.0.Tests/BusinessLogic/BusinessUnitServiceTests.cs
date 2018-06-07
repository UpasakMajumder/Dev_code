using Kadena.BusinessLogic.Services;
using Xunit;

namespace Kadena.Tests.BusinessLogic
{
    public class BusinessUnitServiceTests : KadenaUnitTest<BusinessUnitService>
    {
        [Fact(DisplayName = "BusinessUnitService.GetSiteActiveBusinessUnits()")]
        public void GetSiteActiveBusinessUnits()
        {
            var exc = Record.Exception(() => Sut.GetSiteActiveBusinessUnits());

            Assert.Null(exc);
        }

        [Fact(DisplayName = "BusinessUnitService.GetUserBusinessUnits()")]
        public void GetUserBusinessUnits()
        {
            var exc = Record.Exception(() => Sut.GetUserBusinessUnits(0));

            Assert.Null(exc);
        }
    }
}
