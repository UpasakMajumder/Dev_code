using AutoMapper;
using DryIoc;
using Kadena.Container.Default;
using Kadena.WebAPI.Infrastructure;
using System.Linq;
using Xunit;

namespace Kadena.Tests.Infrastructure
{
    [Trait("Infrastructure", "DIContainer")]
    public class DIContainerTests
    {
        [Fact]
        public void RegisteredServicesResolving()
        {
            // Arrange
            var registeredServices = DIContainer.Instance.GetServiceRegistrations();

            // Act & Assert
            Assert.All(registeredServices, s =>
            {
                var actualResult = DIContainer.Instance.Resolve(s.ServiceType, false);
                Assert.NotNull(actualResult);
            });
        }

        [Fact(DisplayName = "Resolve(IMapper)")]
        public void TestRegisteredMapper()
        {
            // Act
            var result = DIContainer.Resolve<IMapper>();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(MapperBuilder.MapperInstance, result);
        }

        [Fact]
        public void ControllerResolvingTest()
        {
            // Arrange
            var assembly = typeof(ApiControllerBase).GetAssembly();
            var controllers = assembly.GetExportedTypes().Where(t => t.BaseType == typeof(ApiControllerBase)).ToList();

            // Act
            using (var sut = DIContainer.Instance.With(rules => rules.WithoutThrowOnRegisteringDisposableTransient()))
            {
                // Assert
                Assert.All(controllers, (c) =>
                {
                    var actualResult = sut.New(c);
                    Assert.NotNull(actualResult);
                });
            }
        }
    }
}
