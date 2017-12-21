using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceResponses;
using Kadena.Dto.General;
using Kadena2.MicroserviceClients.Clients;
using Kadena2.MicroserviceClients.Contracts.Base;
using Kadena2.WebAPI.KenticoProviders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena2.Carriers
{
    public abstract class CarrierBase : ICarrierProvider
    {
        private readonly IMicroProperties _properties;
        private readonly ShippingCostServiceClient microserviceClient;

        protected string ServiceUrl { get; set; }

        protected string ProviderApiKey { get; set; }

        public string CarrierProviderName
        {
            get; protected set;
        }

        public CarrierBase()
        {
            _properties = ProviderFactory.MicroProperties;
            microserviceClient = new ShippingCostServiceClient(_properties);
            ServiceUrl = _properties.GetServiceUrl("KDA_ShippingCostServiceUrl");
        }

        public Guid GetConfigurationUIElementGUID()
        {
            return Guid.Empty;
        }

        public Guid GetServiceConfigurationUIElementGUID(string serviceName)
        {
            return Guid.Empty;
        }

        protected BaseResponseDto<EstimateDeliveryPricePayloadDto> CallEstimationService(EstimateDeliveryPriceRequestDto requestBody)
        {
            var response = microserviceClient.EstimateShippingCost(requestBody).Result;

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
            var result = CacheHelper.Cache(() => CallEstimationService(requestObject), new CacheSettings(5, cacheKey));
            return result.Success;
        }

        public decimal GetPrice(Delivery delivery, string currencyCode)
        {
            var requestObject = new EstimatePriceRequestFactory().Create(delivery, ProviderApiKey, delivery.ShippingOption.ShippingOptionCarrierServiceName);
            var requestString = microserviceClient.GetRequestString(requestObject);
            string cacheKey = $"estimatedeliveryprice|{ServiceUrl}|{requestString}";
            var result = CacheHelper.Cache(() => CallEstimationService(requestObject), new CacheSettings(5, cacheKey));
            return result.Success ? (decimal)result.Payload?.Cost : 0.0m;
        }

        public abstract List<KeyValuePair<string, string>> GetServices();
    }
}
