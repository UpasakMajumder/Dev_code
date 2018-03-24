using DryIoc;
using Kadena.WebAPI.Infrastructure;
using Kadena.Container.Default;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Kadena.Tests.Infrastructure
{
    [Trait("Infrastructure", "DIContainer")]
    public class ControllerTests
    {
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
