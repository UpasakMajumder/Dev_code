using Kadena.Dto.Product;
using Kadena.Models.Product;
using Kadena.Container.Default;
using Xunit;

namespace Kadena.Tests.Infrastructure
{
    [Trait("Infrastructure", "DIContainer")]
    public class AutomockMapperRegistrationTest
    {
        [Fact(DisplayName = "Mapper.Map<BorderDto>(Border)")]
        public void TestRegisteredMapper()
        {
            // Arrange
            var sut = MapperBuilder.MapperInstance;
            
            // Act
            var result = sut.Map<BorderDto>(new Border { Exists = true, Value = "x" });

            // Assert
            Assert.NotNull(result);
            Assert.Equal("x", result.Value);
            Assert.True(result.Exists);
        }

    }
}
