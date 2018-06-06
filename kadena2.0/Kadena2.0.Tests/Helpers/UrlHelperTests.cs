using Kadena.Helpers;
using Xunit;

namespace Kadena.Tests.Helpers
{
    public class UrlHelperTests
    {
        [Fact]
        public void ParseQueryStringFromUrl_ShouldReturnEmptyCollection_WhenNoParameters()
        {
            const string url = "/some/url?";
            var result = UrlHelper.ParseQueryStringFromUrl(url);
            Assert.Empty(result);
        }

        [Fact]
        public void ParseQueryStringFromUrl_ShouldReturnNonEmptyCollection_WhenUrlContainsParameters()
        {
            const string url = "/some/other/url?param1=value1&param2=value2";
            var result = UrlHelper.ParseQueryStringFromUrl(url);
            Assert.Equal(2, result.Count);
        }
    }
}
