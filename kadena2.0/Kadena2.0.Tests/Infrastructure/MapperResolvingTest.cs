using Kadena.Container.Default;
using Xunit;
using AutoMapper;

namespace Kadena.Tests.Infrastructure
{
    [Trait("Infrastructure", "DIContainer")]
    public class MapperResolvingTest
    {
        [Fact(DisplayName = "Resolve(IMapper)")]
        public void TestRegisteredMapper()
        {
            // Act
            var result = DIContainer.Resolve<IMapper>();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(MapperBuilder.MapperInstance, result);
        }
    }
}
