using Kadena.Models;
using Kadena.WebAPI.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.MicroserviceRequests;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Kadena.WebAPI.Services
{
    public class TaxEstimationService : ITaxEstimationService
    {
        private readonly IKenticoProviderService kenticoProvider;
        private readonly IKenticoLogger kenticoLog;
        private readonly IKenticoResourceService resources;
        private readonly ITaxEstimationServiceClient taxCalculator;
        private readonly ICache cache;

        public TimeSpan ExternalServiceCacheExpiration { get; } = TimeSpan.FromHours(1);

        public TaxEstimationService(IKenticoProviderService kenticoProvider,
                                   IKenticoResourceService resources,                                    
                                   ITaxEstimationServiceClient taxCalculator,
                                   IKenticoLogger kenticoLog,
                                   ICache cache)
        {
            this.kenticoProvider = kenticoProvider;
            this.resources = resources;            
            this.taxCalculator = taxCalculator;            
            this.kenticoLog = kenticoLog;
            this.cache = cache;
        }

        public async Task<decimal> EstimateTotalTax(DeliveryAddress deliveryAddress)
        {
            double totalItemsPrice = kenticoProvider.GetCurrentCartTotalItemsPrice();
            double shippingCosts = kenticoProvider.GetCurrentCartShippingCost();

            if (totalItemsPrice == 0.0d && shippingCosts == 0.0d)
            {
                // no need for tax estimation
                return 0.0m;
            }

            var addressTo = deliveryAddress ?? kenticoProvider.GetCurrentCartShippingAddress();
            var addressFrom = kenticoProvider.GetDefaultBillingAddress();
            var taxRequest = CreateTaxCalculatorRequest(totalItemsPrice, shippingCosts, addressFrom, addressTo);
            var serviceEndpoint = resources.GetSettingsKey("KDA_TaxEstimationServiceEndpoint");

            var estimate = await GetTaxEstimate(serviceEndpoint, taxRequest);

            return estimate;
        }

        private async Task<decimal> GetTaxEstimate(string serviceEndpoint, TaxCalculatorRequestDto taxRequest)
        {
            var cacheKey = GetRequestKey("estimatetaxprice", serviceEndpoint, taxRequest);
            var cachedValue = cache.Get(cacheKey);
            if (cachedValue != null)
            {
                return (decimal)cachedValue;
            }

            var response = await taxCalculator.CalculateTax(serviceEndpoint, taxRequest);
            if (response.Success)
            {
                var value = response.Payload;
                cache.Insert(cacheKey, value, DateTime.UtcNow.Add(ExternalServiceCacheExpiration));
                return value;
            }
            else
            {
                kenticoLog.LogError("DeliveryPriceEstimationClient", $"Call for tax estimation to service URL '{serviceEndpoint}' resulted with error {response.Error?.Message ?? string.Empty}");
                return 0.0m;
            }
        }

        private string GetRequestKey(string keyPrefix, string serviceEndpoint, TaxCalculatorRequestDto taxRequest)
        {
            var requestString = JsonConvert.SerializeObject(taxRequest);
            return $"{keyPrefix}|{serviceEndpoint}|{taxRequest}";
        }

        private TaxCalculatorRequestDto CreateTaxCalculatorRequest(double totalItemsPrice, double shippingCosts, BillingAddress addressFrom, DeliveryAddress addressTo)
        {
            var taxRequest = new TaxCalculatorRequestDto()
            {
                TotalBasePrice = totalItemsPrice,
                ShipCost = shippingCosts
            };

            if (addressFrom != null)
            {
                taxRequest.ShipFromCity = addressFrom.City ?? string.Empty;
                taxRequest.ShipFromState = addressFrom.State ?? string.Empty;
                taxRequest.ShipFromZip = addressFrom.Zip ?? string.Empty;
            }

            if (addressTo != null)
            {
                taxRequest.ShipToCity = addressTo.City ?? string.Empty;
                taxRequest.ShipToState = addressTo.State ?? string.Empty;
                taxRequest.ShipToZip = addressTo.Zip ?? string.Empty;
            }

            return taxRequest;
        }				
    }
}
 