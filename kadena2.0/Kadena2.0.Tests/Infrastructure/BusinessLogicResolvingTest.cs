using Kadena.Container.Default;
using Xunit;

namespace Kadena.Tests.Infrastructure
{
    [Trait("Infrastructure", "DIContainer")]
    public class BusinessLogicResolvingTest
    {
        [Fact]
        public void BusinessLogicInterfacesResolvable()
        {
            // Arrange
            var registeredServices = DIContainer.Instance.GetServiceRegistrations();

            // Act & Assert
            Assert.All(registeredServices, s => {
                var actualResult = DIContainer.Instance.Resolve(s.ServiceType, false);
                Assert.NotNull(actualResult);
            });
        }
    }
}
