using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.SiteProvider;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceResponses;
using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Clients;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena2.Carriers
{
    public abstract class CarrierBase : ICarrierProvider
    {
        private ShippingCostServiceClient microserviceClient = new ShippingCostServiceClient();

        protected string ServiceUrl { get; set; }

        protected string ProviderApiKey { get; set; }

        public string CarrierProviderName
        {
            get; protected set;
        }

        public CarrierBase()
        {
            ServiceUrl = SettingsKeyInfoProvider.GetValue($"{SiteContext.CurrentSiteName}.KDA_EstimateDeliveryPriceServiceEndpoint");
        }

        public Guid GetConfigurationUIElementGUID()
        {
            return Guid.Empty;
        }

        public Guid GetServiceConfigurationUIElementGUID(string serviceName)
        {
            return Guid.Empty;
        }

        protected BaseResponseDto<EstimateDeliveryPricePayloadDto> CallEstimationService(string requestBody)
        {
            var response = microserviceClient.EstimateShippingCost(ServiceUrl, requestBody).Result;

            if (!response.Success || response.Payload == null)
            {
                EventLogProvider.LogInformation("DeliveryPriceEstimationClient", "ERROR", $"Call from '{CarrierProviderName}' provider to service URL '{ServiceUrl}' resulted with error {response.Error?.Message ?? string.Empty}");
            }

            return response;
        }

        public bool CanDeliver(Delivery delivery)
        {
            if (delivery.Items.Count() == 0 ||
                delivery.DeliveryAddress == null)
                return false;

            var requestObject = new EstimatePriceRequestFactory().Create(delivery, ProviderApiKey, delivery.ShippingOption.ShippingOptionCarrierServiceName);
            var requestString = microserviceClient.GetRequestString(requestObject);
            string cacheKey = $"estimatedeliveryprice|{ServiceUrl}|{requestString}";
            var result = CacheHelper.Cache<BaseResponseDto<EstimateDeliveryPricePayloadDto>>(() => CallEstimationService(requestString), new CacheSettings(5, cacheKey));
            return result.Success;
        }

        public decimal GetPrice(Delivery delivery, string currencyCode)
        {
            var requestObject = new EstimatePriceRequestFactory().Create(delivery, ProviderApiKey, delivery.ShippingOption.ShippingOptionCarrierServiceName);
            var requestString = microserviceClient.GetRequestString(requestObject);
            string cacheKey = $"estimatedeliveryprice|{ServiceUrl}|{requestString}";
            var result = CacheHelper.Cache<BaseResponseDto<EstimateDeliveryPricePayloadDto>>(() => CallEstimationService(requestString), new CacheSettings(5, cacheKey));
            return result.Success ? (decimal)result.Payload?.Cost : 0.0m;
        }

        public abstract List<KeyValuePair<string, string>> GetServices();
    }
}
