using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.SiteProvider;
using Kadena2.Carriers.ServiceApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Kadena2.Carriers
{
    public abstract class CarrierBase : ICarrierProvider
    {
        protected string ServiceUrl { get; set; }

        protected string ProviderApiKey { get; set; }

        public string CarrierProviderName
        {
            get; protected set;
        }

        public CarrierBase()
        {
            ServiceUrl = SettingsKeyInfoProvider.GetValue( SiteContext.CurrentSiteName+".KDA_EstimateDeliveryPriceServiceEndpoint");
        }

        public Guid GetConfigurationUIElementGUID()
        {
            return Guid.Empty;
        }

        public Guid GetServiceConfigurationUIElementGUID(string serviceName)
        {
            return Guid.Empty;
        }

        protected EstimateDeliveryPriceResponse CallEstimationService(string requestBody)
        {
            using (var httpClient = new HttpClient())
            {
                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var response = httpClient.PostAsync(ServiceUrl, content).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string responseContent = response.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<EstimateDeliveryPriceResponse>(responseContent);
                }
                else
                {
                    return new EstimateDeliveryPriceResponse() { success = false, payload = null };
                }
            }
        }

        public bool CanDeliver(Delivery delivery)
        {
            if (delivery.Items.Count() == 0 ||
                delivery.DeliveryAddress == null)
                return false;

            var requestObject = new EstimatePriceRequestFactory().Create(delivery, ProviderApiKey, delivery.ShippingOption.ShippingOptionCarrierServiceName);
            var requestString = JsonConvert.SerializeObject(requestObject);
            var result = CacheHelper.Cache<EstimateDeliveryPriceResponse>(() => CallEstimationService(requestString), new CacheSettings(10, $"estimatedeliveryprice|{requestString}"));
            return result.success;
        }

        public decimal GetPrice(Delivery delivery, string currencyCode)
        {
            var requestObject = new EstimatePriceRequestFactory().Create(delivery, ProviderApiKey, delivery.ShippingOption.ShippingOptionCarrierServiceName);
            var requestString = JsonConvert.SerializeObject(requestObject);
            var result = CacheHelper.Cache<EstimateDeliveryPriceResponse>(() => CallEstimationService(requestString), new CacheSettings(10, $"estimatedeliveryprice|{requestString}"));
            return result.success ? (decimal)result.payload?.cost : 0.0m;
        }

        public abstract List<KeyValuePair<string, string>> GetServices();
    }
}
