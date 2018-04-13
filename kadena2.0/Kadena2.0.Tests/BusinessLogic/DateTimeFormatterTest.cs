using Xunit;
using System;
using Kadena.BusinessLogic.Services;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.Tests.BusinessLogic
{
    public class DateTimeFormatterTest : KadenaUnitTest<DateTimeFormatter>
    {
        [Theory(DisplayName = "DateTimeFormatter.Format()")]
        [InlineData("Jan 30 2017", "en-US", "Jan 30, 2017")]
        [InlineData("Jan 09 2017", "en-US", "Jan 09, 2017")]
        [InlineData("Jan 30 2017", "ja-JP", "1月 30, 2017")]
        [InlineData("Dec 30 2017", "ja-JP", "12月 30, 2017")]
        public void FormatterTest(string date, string culture, string expectedOutput)
        {
            // Arrange
            Setup<IKenticoLocalizationProvider, string>(m => m.GetContextCultureCode(), culture);

            // Act
            var outputDate = Sut.Format(DateTime.Parse(date));

            // Assert
            Assert.Equal(expectedOutput, outputDate);
        }

    }
}
