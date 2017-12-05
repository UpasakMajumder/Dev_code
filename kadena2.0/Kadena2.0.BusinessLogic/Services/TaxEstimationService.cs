using Kadena.Models;
using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.MicroserviceRequests;
using System.Linq;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services
{
    public class TaxEstimationService : ITaxEstimationService
    {
        private readonly IKenticoProviderService kenticoProvider;
        private readonly IKenticoLogger kenticoLog;
        private readonly IKenticoResourceService resources;
        private readonly ITaxEstimationServiceClient taxCalculator;
        private readonly ICache cache;

        public string ServiceEndpoint => resources.GetSettingsKey("KDA_TaxEstimationServiceEndpoint");

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
                kenticoLog.LogError("DeliveryPriceEstimationClient", $"Call for tax estimation to service URL '{serviceEndpoint}' resulted with error {response.Error?.Message ?? string.Empty}");
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

            var stateTo = kenticoProvider.GetStates().FirstOrDefault(s => s.Id == (addressTo?.State?.Id ?? 0));

            if (addressFrom != null)
            {
                taxRequest.ShipFromCity = addressFrom.City ?? string.Empty;
                taxRequest.ShipFromState = addressFrom.State ?? string.Empty;
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
            double totalItemsPrice = kenticoProvider.GetCurrentCartTotalItemsPrice();
            double shippingCosts = kenticoProvider.GetCurrentCartShippingCost();

            var addressTo = deliveryAddress ?? kenticoProvider.GetCurrentCartShippingAddress();
            var addressFrom = kenticoProvider.GetDefaultBillingAddress();

            return CreateTaxEstimationRequest(totalItemsPrice, shippingCosts, addressFrom, addressTo);
        }
    }
}
 