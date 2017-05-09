using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.Helpers;
using Kadena2.Carriers.ServiceApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            ServiceUrl = SettingsKeyInfoProvider.GetValue("KDA_EstimateDeliveryPriceServiceEndpoint");
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

        protected EstimateDeliveryPriceRequest GetEstimatePriceRequest(Delivery delivery, string provider, string service)
        {
            return new EstimateDeliveryPriceRequest()
            {
                provider = provider,
                providerService = service,
                sourceAddress = new Address()
                {
                    streetLines = new List<string>() { "SHIPPER ADDRESS LINE 1" },
                    city = "COLLIERVILLE",
                    state = "TN",
                    postal = "38017",
                    country = "US"
                },
                targetAddress = GetAddress(delivery.DeliveryAddress),
                weight = new Weight() { unit = "Lb", value = (double)delivery.Weight }
            };
        }

        private Address GetAddress(IAddress address)
        {
            if (address == null)
                return new Address();

            return new Address()
            {
                city = address.AddressCity,
                country = address.GetCountryTwoLetterCode(),
                postal = address.AddressZip,
                state = address.GetStateCode(),
                streetLines = new List<string> { address.AddressLine1, address.AddressLine2 }
            };
        }

        public bool CanDeliver(Delivery delivery)
        {
            var requestObject = GetEstimatePriceRequest(delivery, ProviderApiKey, delivery.ShippingOption.ShippingOptionCarrierServiceName);
            var requestString = JsonConvert.SerializeObject(requestObject);
            var result = CacheHelper.Cache<EstimateDeliveryPriceResponse>(() => CallEstimationService(requestString), new CacheSettings(10, $"estimatedeliveryprice|{requestString}"));
            return result.success;
        }

        public decimal GetPrice(Delivery delivery, string currencyCode)
        {
            var requestObject = GetEstimatePriceRequest(delivery, ProviderApiKey, delivery.ShippingOption.ShippingOptionCarrierServiceName);
            var requestString = JsonConvert.SerializeObject(requestObject);
            var result = CacheHelper.Cache<EstimateDeliveryPriceResponse>(() => CallEstimationService(requestString), new CacheSettings(10, $"estimatedeliveryprice|{requestString}"));
            return (decimal)result.payload.cost;
        }

        public abstract List<KeyValuePair<string, string>> GetServices();
    }
}
