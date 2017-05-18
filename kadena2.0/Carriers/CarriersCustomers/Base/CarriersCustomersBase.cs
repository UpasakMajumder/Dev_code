using CMS.Ecommerce;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena2.Carriers
{
    public abstract class CarriersCustomersBase : ICarrierProvider
    {
        public string CarrierProviderName
        {
            get; protected set;            
        }

        public CarriersCustomersBase()
        {
            
        }

        public Guid GetConfigurationUIElementGUID()
        {
            return Guid.Empty;
        }

        public Guid GetServiceConfigurationUIElementGUID(string serviceName)
        {
            return Guid.Empty;
        }

        public bool CanDeliver(Delivery delivery)
        {
            return true;
        }

        public decimal GetPrice(Delivery delivery, string currencyCode)
        {
            return 0.0m;
        }

        public List<KeyValuePair<string, string>> GetServices()
        {
            var services = new SortedDictionary<string, string>
            {
                {$"{CarrierProviderName}_CUSTOMER_PRICE", $"{CarrierProviderName} customer price"}
            };

            return services.ToList();
        }
    }
}
