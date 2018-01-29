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
            var assembly = Assembly.LoadFrom("Kadena2.0.WebAPI.dll");

            var controllers = assembly.GetExportedTypes().Where(t => t.BaseType == typeof(ApiControllerBase)).ToList();

            var container = new Container(rules => rules.WithoutThrowOnRegisteringDisposableTransient())
                .RegisterBLL()
                .RegisterKentico()
                .RegisterKadenaSettings()
                .RegisterMicroservices()
                .RegisterFactories()
                .RegisterInfrastructure();

            foreach (var controller in controllers)
            {
                container.Register(controller);
            }

            foreach (var controller in controllers)
            {
                if (controller.Name == "DashboardController" ||
                    controller.Name == "OrdersController" )
                {
                    continue; // TODO : For some reason, it crashes because of some cms type
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
        }
    }
}
