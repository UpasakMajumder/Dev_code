using Kadena.Models;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.MicroserviceRequests;
using System.Linq;
using System.Threading.Tasks;
using System;
using Kadena2.Infrastructure.Contracts;

namespace Kadena.BusinessLogic.Services
{
    public class TaxEstimationService : ITaxEstimationService
    {
        private readonly IKenticoLocalizationProvider kenticoLocalization;
        private readonly IKenticoLogger kenticoLog;
        private readonly IKenticoResourceService resources;
        private readonly ITaxEstimationServiceClient taxCalculator;
        private readonly IShoppingCartProvider shoppingCart;
        private readonly ICache cache;

        public string ServiceEndpoint => resources.GetSiteSettingsKey("KDA_TaxEstimationServiceEndpoint");

        public TaxEstimationService(IKenticoLocalizationProvider kenticoLocalization,
                                   IKenticoResourceService resources,                                    
                                   ITaxEstimationServiceClient taxCalculator,
                                   IKenticoLogger kenticoLog,
                                   IShoppingCartProvider shoppingCart,
                                   ICache cache)
        {
            if (kenticoLocalization == null)
            {
                throw new ArgumentNullException(nameof(kenticoLocalization));
            }
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }
            if (taxCalculator == null)
            {
                throw new ArgumentNullException(nameof(taxCalculator));
            }
            if (kenticoLog == null)
            {
                throw new ArgumentNullException(nameof(kenticoLog));
            }
            if (shoppingCart == null)
            {
                throw new ArgumentNullException(nameof(shoppingCart));
            }
            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            this.kenticoLocalization = kenticoLocalization;
            this.resources = resources;            
            this.taxCalculator = taxCalculator;            
            this.kenticoLog = kenticoLog;
            this.shoppingCart = shoppingCart;
            this.cache = cache;
        }

        public async Task<decimal> EstimateTotalTax(DeliveryAddress deliveryAddress)
        {
            var taxRequest = CreateTaxEstimationRequest(deliveryAddress);

            var estimate = await EstimateTotalTax(ServiceEndpoint, taxRequest);
            return estimate;
        }

        private async Task<decimal> EstimateTotalTax(string serviceEndpoint, TaxCalculatorRequestDto taxRequest)
        {
            if (taxRequest.TotalBasePrice == 0.0d && taxRequest.ShipCost == 0.0d)
            {
                // no need for tax estimation
                return 0.0m;
            }

            var cacheKey = $"DeliveryPriceEstimationClient|{serviceEndpoint}|{Newtonsoft.Json.JsonConvert.SerializeObject(taxRequest)}";
            var cachedResult = cache.Get(cacheKey);
            if (cachedResult != null)
            {
                return (decimal)cachedResult;
            }

            var response = await taxCalculator.CalculateTax(taxRequest);
            if (response.Success)
            {
                var result = response.Payload;
                cache.Insert(cacheKey, result);
                return result;
            }
            else
            {
                kenticoLog.LogError("TaxCalculatorClient", $"Call for tax estimation to service URL '{serviceEndpoint}' resulted with error {response.Error?.Message ?? string.Empty}");
                return 0.0m;
            }
        }

        private TaxCalculatorRequestDto CreateTaxEstimationRequest(double totalItemsPrice, double shippingCosts, BillingAddress addressFrom, DeliveryAddress addressTo)
        {
            var taxRequest = new TaxCalculatorRequestDto()
            {
                TotalBasePrice = totalItemsPrice,
                ShipCost = shippingCosts
            };

            var stateTo = kenticoLocalization.GetStates().FirstOrDefault(s => s.Id == (addressTo?.State?.Id ?? 0));

            if (addressFrom != null)
            {
                taxRequest.ShipFromCity = addressFrom.City ?? string.Empty;
                taxRequest.ShipFromState = addressFrom.State?.StateCode ?? string.Empty;
                taxRequest.ShipFromZip = addressFrom.Zip ?? string.Empty;
            }

            if (addressTo != null)
            {
                taxRequest.ShipToCity = addressTo.City ?? string.Empty;
                taxRequest.ShipToState = stateTo?.StateCode ?? string.Empty;
                taxRequest.ShipToZip = addressTo.Zip ?? string.Empty;
            }

            return taxRequest;
        }

        private TaxCalculatorRequestDto CreateTaxEstimationRequest(DeliveryAddress deliveryAddress)
        {
            double totalItemsPrice = shoppingCart.GetCurrentCartTotalItemsPrice();
            double shippingCosts = shoppingCart.GetCurrentCartShippingCost();

            var addressTo = deliveryAddress ?? shoppingCart.GetCurrentCartShippingAddress();
            var addressFrom = shoppingCart.GetDefaultBillingAddress();

            return CreateTaxEstimationRequest(totalItemsPrice, shippingCosts, addressFrom, addressTo);
        }
    }
}
 