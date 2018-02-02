using Kadena2.Container.Default;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Kadena.Tests.Infrastructure
{
    [Trait("Infrastructure", "DIContainer")]
    public class KenticoProvidersResolvingTest
    {
        [Fact]
        public void KenticoProvidersInterfacesResolvable()
        {
            // Arrange
            var assembly = Assembly.LoadFrom("Kadena2.0.WebAPI.KenticoProviders.dll");
            var services = assembly.GetExportedTypes().Where(t => t.IsInterface).ToList();
            
            // Act & Assert
            foreach (var service in services)
            {
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
    }
}
