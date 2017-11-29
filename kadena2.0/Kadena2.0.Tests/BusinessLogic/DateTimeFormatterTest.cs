using Xunit;
using Moq;
using System;
using Kadena.BusinessLogic.Services;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.Site;

namespace Kadena.Tests.BusinessLogic
{
    public class DateTimeFormatterTest
    {
        [Theory]
        [InlineData("Jan 30 2017", "en-US", "Jan 30, 2017")]
        [InlineData("Jan 09 2017", "en-US", "Jan 09, 2017")]
        [InlineData("Jan 30 2017", "ja-JP", "1月 30, 2017")]
        [InlineData("Dec 30 2017", "ja-JP", "12月 30, 2017")]
        [InlineData("Dec 30 2017", "cs-CZ", "pro 30, 2017")]
        public void FormatterTest(string date, string culture, string expectedOutput)
        {
            // Arrange
            var datetime = DateTime.Parse(date);
            var resources = new Mock<IKenticoResourceService>();
            resources.Setup(m => m.GetContextCultureCode())
                .Returns(culture);
            var dateFormatter = new DateTimeFormatter(resources.Object);

            // Act
            var outputDate = dateFormatter.Format(datetime);

            // Assert
            Assert.Equal(expectedOutput, outputDate);
        }

    }
}
