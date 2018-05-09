using AutoMapper;
using DryIoc;
using Kadena.Container.Default;
using Kadena.WebAPI.Infrastructure;
using System;
using System.Linq;
using System.Reflection;
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
            Assert.All(registeredServices, s => {
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
            var assembly = Assembly.LoadFrom("Kadena2.0.WebAPI.dll");
            var controllers = assembly.GetExportedTypes().Where(t => t.BaseType == typeof(ApiControllerBase)).ToList();
            var container = DIContainer.Instance.With(rules => rules.WithoutThrowOnRegisteringDisposableTransient());
            controllers.ForEach(c => container.Register(c, ifAlreadyRegistered: IfAlreadyRegistered.Keep));
            
            // Act
            foreach (var controller in controllers)
            {
                if (controller.Name == "DashboardController" || controller.Name == "RecentOrdersController")
                {
                    continue; // because setter of 'public string PageCapacityKey' in 'OrderListService' is looking for setting key into Kentico
                }

                try
                {
                    var instance = container.Resolve(controller);

                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to resolve {controller.Name}", ex);
                }
            }

            // Assert
            // this test should not affect original container:
            Assert.Throws<ContainerException>(() => DIContainer.Instance.Resolve(controllers[0]));
            Assert.Throws<ContainerException>(() => DIContainer.Instance.Resolve<INotRegisteredSubservice>());
        }
    }
}
