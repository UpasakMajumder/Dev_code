using Kadena.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kadena.Tests.Helpers
{
    public class UrlHelperTests
    {
        [Theory]
        [InlineData("/some/url", "param", "value", "/some/url?param=value")]
        [InlineData("/some/url?param3=value3", "param", "value", "/some/url?param3=value3&param=value")]
        [InlineData("/some/url?param=value", "param", "value2", "/some/url?param=value2")]
        [InlineData(null, "param", "value2", "?param=value2")]
        [InlineData("asdf", "param", "value2", "asdf?param=value2")]
        public void SetQueryParameter_ShouldAddOrUpdateParameter(string oldUrl, string parameterName, string newValue, string newUrl)
        {
            var result = UrlHelper.SetQueryParameter(oldUrl, parameterName, newValue);
            Assert.Equal(newUrl, result);
        }

        [Fact]
        public void SetQueryParameters_ShouldAddOrUpdateParameters()
        {
            var oldUrl = "/some/url";
            var newUrl = "/some/url?param1=val1&param2=val2";
            var result = UrlHelper.SetQueryParameters(oldUrl, new[] 
            {
                new KeyValuePair<string, string>("param1", "val1"),
                new KeyValuePair<string, string>("param2", "val2"),
            });
            Assert.Equal(newUrl, result);
        }

        [Theory]
        [InlineData("/some/url", "/some/url")]
        [InlineData("/some/url?param1=asdf", "/some/url")]
        [InlineData("http://lost.com/some/url", "/some/url")]
        [InlineData("http://lost.com/some/url?param1=adsf", "/some/url")]
        [InlineData("?param1=adsf", "/")]
        [InlineData("/", "/")]
        [InlineData("", "/")]
        public void GetPathFromUrl_ShouldExtractPathPartFromUrl(string url, string path)
        {
            var result = UrlHelper.GetPathFromUrl(url);

            Assert.Equal(path, result);
        }

        [Theory]
        [InlineData("/some/url", "")]
        [InlineData("/some/url?param1=asdf", "param1=asdf")]
        [InlineData("?param1=adsf", "param1=adsf")]
        [InlineData("/", "")]
        [InlineData("", "")]
        public void GetQueryFromUrl_ShouldExtractQueryPartFromUrl(string url, string query)
        {
            var result = UrlHelper.GetQueryFromUrl(url);

            Assert.Equal(query, result);
        }
    }
}
