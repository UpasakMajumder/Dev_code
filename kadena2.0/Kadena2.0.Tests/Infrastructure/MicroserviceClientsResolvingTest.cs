using Kadena2.Container.Default;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Kadena.Tests.Infrastructure
{
    [Trait("Infrastructure", "DIContainer")]
    public class MicroserviceClientsResolvingTest
    {
        [Fact]
        public void MicroserviceClientsInterfacesResolvable()
        {
            // Arrange
            var assembly = Assembly.LoadFrom("Kadena2.0.MicroserviceClients.dll");
            var services = assembly.GetExportedTypes().Where(t => t.IsInterface).ToList();
            
            // Act & Assert
            foreach (var service in services)
            {
                if (service.Name == "ICloudEventConfiguratorClient" ||
                    service.Name == "IParsingClient" ||
                    service.Name == "IStatisticsClient" ||
                    service.Name == "IExportClient")
                {
                    continue; // TODO check why it is not registered/resolved, 
                              // maybe because of using only in webforms ?
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
    }
}
