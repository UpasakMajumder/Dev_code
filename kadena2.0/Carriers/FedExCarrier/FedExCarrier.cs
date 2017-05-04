using System;
using System.Collections.Generic;
using System.Linq;
using CMS;
using CMS.Ecommerce;
using Kadena2.FedExCarrier;
using Kadena2.CarrierBase;
using CMS.Helpers;
using CMS.Base;
using CMS.DataEngine;
using Newtonsoft.Json;

[assembly: AssemblyDiscoverable]
[assembly: RegisterCustomClass("FedExCarrier", typeof(FedExCarrier))]

namespace Kadena2.FedExCarrier
{
    public class FedExCarrier : CarrierBase.CarrierBase, ICarrierProvider
    {

        public string CarrierProviderName
        {
            get
            {
                return "Fedex";
            }
        }
        public FedExCarrier() : base()
        {
            ProviderApiKey = "Fedex";
        }

        public bool CanDeliver(Delivery delivery)
        {
            var requestObject = GetEstimatePriceRequest(delivery, ProviderApiKey, GetServices()[0].Key);
            var requestString = JsonConvert.SerializeObject(requestObject);
            var result = CacheHelper.Cache<EstimateDeliveryPriceResponse>(() => CallEstimationService(requestString), new CacheSettings(10, $"estimatedeliveryprice|{requestString}"));
            return result.success && result.response.serviceSuccess;
        }

        public Guid GetConfigurationUIElementGUID()
        {
            return Guid.Empty;
        }

        public decimal GetPrice(Delivery delivery, string currencyCode)
        {
            var requestObject = GetEstimatePriceRequest(delivery, ProviderApiKey, GetServices()[0].Key);
            var requestString = JsonConvert.SerializeObject(requestObject);
            var result = CacheHelper.Cache<EstimateDeliveryPriceResponse>(() => CallEstimationService(requestString), new CacheSettings(10, $"estimatedeliveryprice|{requestString}"));
            return (decimal)result.response.cost;
        }

        public Guid GetServiceConfigurationUIElementGUID(string serviceName)
        {
            return Guid.Empty;
        }

        public List<KeyValuePair<string, string>> GetServices()
        {
            var services = new SortedDictionary<string, string>
            {
                {"PRIORITY_OVERNIGHT", "Priority overnight"},
                {"STANDARD_OVERNIGHT", "Standard overnight"},
                {"FEDEX_2_DAY", "Fedex 2nd day"}
            };

            return services.ToList();
        }
    }
}
