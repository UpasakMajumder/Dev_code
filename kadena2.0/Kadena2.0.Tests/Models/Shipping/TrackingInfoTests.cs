using Kadena.Models.Shipping;
using Xunit;

namespace Kadena.Tests.Models.Shipping
{
    public class TrackingInfoTests : KadenaUnitTest<TrackingInfo>
    {
        [Theory(DisplayName = "TrackingInfo.ToString()")]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("testdata1")]
        [InlineData("testdata2")]
        public void ToStringEqualId(string expected)
        {
            var sut = Sut;
            sut.Id = expected;

            var actualResult = sut.ToString();

            Assert.Equal(expected, actualResult);
        }
    }
}
