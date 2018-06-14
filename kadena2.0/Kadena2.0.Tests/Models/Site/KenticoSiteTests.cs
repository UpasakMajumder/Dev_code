using Xunit;
using Kadena.Models.Site;

namespace Kadena.Tests.Models.Site
{
    public class KenticoSiteTests : KadenaUnitTest<KenticoSite>
    {
        [Theory(DisplayName = "KenticoSite.ToString()")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("testdata1")]
        [InlineData("testdata2")]
        public void ToStringEqualName(string expected)
        {
            var sut = Sut;
            sut.Name = expected;

            var actualResult = sut.ToString();

            Assert.Equal(expected, actualResult);
        }
    }
}
