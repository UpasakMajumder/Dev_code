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

        public TaxEstimationService(IKenticoLocalizationProvider kenticoLocalization,
                                   IKenticoResourceService resources,                                    
                                   ITaxEstimationServiceClient taxCalculator,
                                   IKenticoLogger kenticoLog,
                                   IShoppingCartProvider shoppingCart,
                                   ICache cache)
        {
            this.kenticoLocalization = kenticoLocalization ?? throw new ArgumentNullException(nameof(kenticoLocalization));
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));            
            this.taxCalculator = taxCalculator ?? throw new ArgumentNullException(nameof(taxCalculator));            
            this.kenticoLog = kenticoLog ?? throw new ArgumentNullException(nameof(kenticoLog));
            this.shoppingCart = shoppingCart ?? throw new ArgumentNullException(nameof(shoppingCart));
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<decimal> EstimateTax(DeliveryAddress deliveryAddress, double pricedItemsPrice, double shippingCost)
        {
            var taxRequest = CreateTaxEstimationRequest(deliveryAddress, pricedItemsPrice, shippingCost);

            var estimate = await EstimateTotalTaxCachedCall(taxRequest);
            return estimate;
        }

        private async Task<decimal> EstimateTotalTaxCachedCall(TaxCalculatorRequestDto taxRequest)
        {
            if (taxRequest.TotalBasePrice == 0.0d && taxRequest.ShipCost == 0.0d)
            {
                // no need for tax estimation
                return 0.0m;
            }

            var cacheKey = $"TaxEstimationClient|{Newtonsoft.Json.JsonConvert.SerializeObject(taxRequest)}";
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
                kenticoLog.LogError("TaxCalculatorClient", $"Call for tax estimation resulted with error {response.Error?.Message ?? string.Empty}");
                return 0.0m;
            }
        }

        private TaxCalculatorRequestDto CreateTaxEstimationRequest(DeliveryAddress deliveryAddress, double totalItemsPrice, double shippingCosts)
        {        
            var addressTo = deliveryAddress ?? shoppingCart.GetCurrentCartShippingAddress();
            var addressFrom = shoppingCart.GetDefaultBillingAddress();

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
    }
}
 