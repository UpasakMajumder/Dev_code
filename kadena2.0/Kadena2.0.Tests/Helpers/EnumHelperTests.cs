using Xunit;
using Kadena.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Kadena.Tests.Helpers
{
    public class EnumHelperTests
    {
        const string expectedDisplay = "Some display value";

        enum TestEnum
        {
            WithoutDisplayAttribute,

            [Display(Name = expectedDisplay)]
            NormalDisplayAttribute,

            [Display]
            EmptyDisplayAttribute,
        }

        [Fact]
        public void GetDisplayName_Empty()
        {
            var actualResult = TestEnum.EmptyDisplayAttribute.GetDisplayName();

            Assert.Null(actualResult);
        }

        [Fact]
        public void GetDisplayName_Specified()
        {
            var actualResult = TestEnum.NormalDisplayAttribute.GetDisplayName();

            Assert.Equal(expectedDisplay, actualResult);
        }

        [Fact]
        public void GetDisplayName_NotSpecified()
        {
            var actualResult = TestEnum.WithoutDisplayAttribute.GetDisplayName();

            Assert.Equal(string.Empty, actualResult);
        }
    }
}
