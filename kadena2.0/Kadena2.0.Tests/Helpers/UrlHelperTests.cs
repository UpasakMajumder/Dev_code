using Kadena.Helpers;
using System;
using Xunit;

namespace Kadena.Tests.Helpers
{
    public class UrlHelperTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ParseQueryStringFromUrl_ShouldReturnEmptyCollection_WhenEmptyParameters(string url)
        {
            var result = UrlHelper.ParseQueryStringFromUrl(url);
            Assert.Empty(result);
        }

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

        [Theory]
        [InlineData("/some/url", "param", "value", "/some/url?param=value")]
        [InlineData("/some/url?param3=value3", "param", "value", "/some/url?param3=value3&param=value")]
        [InlineData("/some/url?param=value", "param", "value2", "/some/url?param=value2")]
        public void SetQueryParameter_ShouldAddOrUpdateParameter(string oldUrl, string parameterName, string newValue, string newUrl)
        {
            var result = UrlHelper.SetQueryParameter(oldUrl, parameterName, newValue);
            Assert.Equal(newUrl, result);
        }

        [Fact]
        public void SetQueryParameter_ShouldThrow_WhenUrlIsNull()
        {
            Action action = () => UrlHelper.SetQueryParameter(null, "param1", "value1");
            Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void SetQueryParameter_ShouldThrow_WhenParameterNameIsNull()
        {
            Action action = () => UrlHelper.SetQueryParameter("", null, "value1");
            Assert.Throws<ArgumentNullException>(action);
        }
    }
}
