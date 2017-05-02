using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS;
using CMS.Ecommerce;

[assembly: AssemblyDiscoverable]

namespace FedExCarrier
{
    [assembly: RegisterCustomClass("MyCustomCarrier", typeof(MyCustomCarrier))]
    public class FedExCarrier : ICarrierProvider
    {
        public string CarrierProviderName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool CanDeliver(Delivery delivery)
        {
            throw new NotImplementedException();
        }

        public Guid GetConfigurationUIElementGUID()
        {
            throw new NotImplementedException();
        }

        public decimal GetPrice(Delivery delivery, string currencyCode)
        {
            throw new NotImplementedException();
        }

        public Guid GetServiceConfigurationUIElementGUID(string serviceName)
        {
            throw new NotImplementedException();
        }

        public List<KeyValuePair<string, string>> GetServices()
        {
            throw new NotImplementedException();
        }
    }
}
