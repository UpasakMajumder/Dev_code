using DryIoc;
using Kadena.WebAPI.Infrastructure;
using Kadena2.Container.Default;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Kadena.Tests.Infrastructure
{
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
                if (controller.Name == "DashboardController" ||
                       controller.Name == "OrdersController" )
                {
                       continue; // TODO : For some reason, it crashes because of 
                                 // System.InvalidOperationException : Object type 'cms.culture' not found.
                                 // (maybe reference kentico dlls from test project)
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
        }

        
    }
}
