using Kadena.AmazonFileSystemProvider;
using Kadena.BusinessLogic.Contracts;
using Kadena.Container.Default;
using System;
using System.Linq;
using System.Reflection;
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
            var assembly = Assembly.LoadFrom("Kadena2.0.BusinessLogic.dll");
            var services = assembly.GetExportedTypes().Where(t => t.IsInterface).ToList();

            // Act & Assert
            foreach (var service in services)
            {
                if (service.Name == nameof(IOrderListService))
                {
                    // this is resolved using factory
                    continue; 
                }

                try
                {
                    DIContainer.Instance.Resolve(service, false);

                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to resolve {service.Name}", ex);
                }
            }
        }

        [Fact]
        public void ResolveIS3PathService()
        {
            var actualResult = DIContainer.Resolve<IS3PathService>();

            Assert.NotNull(actualResult);
        }
    }
}
