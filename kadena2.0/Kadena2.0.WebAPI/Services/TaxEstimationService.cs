using Kadena.WebAPI.Contracts;
using Kadena.Models;
using System.Threading.Tasks;
using Kadena2.MicroserviceClients.Contracts;
using Kadena2.MicroserviceClients.MicroserviceRequests;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.WebAPI.Services
{
    public class TaxEstimationService : ITaxEstimationService
    {
        private readonly IKenticoProviderService kenticoProvider;
        private readonly IKenticoLogger kenticoLog;
        private readonly IKenticoResourceService resources;
        private readonly ITaxEstimationServiceClient taxCalculator;

        public TaxEstimationService(IKenticoProviderService kenticoProvider,
                                   IKenticoResourceService resources,                                    
                                   ITaxEstimationServiceClient taxCalculator,
                                   IKenticoLogger kenticoLog)
        {
            this.kenticoProvider = kenticoProvider;
            this.resources = resources;            
            this.taxCalculator = taxCalculator;            
            this.kenticoLog = kenticoLog;
        }

        public async Task<double> EstimateTotalTax(DeliveryAddress deliveryAddress)
        {
            DeliveryAddress addressTo = null;

            if (deliveryAddress != null)
            {
                addressTo = deliveryAddress;
            }
            else
            {
                addressTo = kenticoProvider.GetCurrentCartShippingAddress();
            }

            var addressFrom = kenticoProvider.GetDefaultBillingAddress();
            var serviceEndpoint = resources.GetSettingsKey("KDA_TaxEstimationServiceEndpoint");
            double totalItemsPrice = kenticoProvider.GetCurrentCartTotalItemsPrice();
            double shippingCosts = kenticoProvider.GetCurrentCartShippingCost();

            if (totalItemsPrice == 0.0d && shippingCosts == 0.0d)
            {
                // not call microservice in this case
                return 0.0d;
            }

            var taxRequest = CreateTaxCalculatorRequest(totalItemsPrice, shippingCosts, addressFrom, addressTo);
            var taxResponse = await taxCalculator.CalculateTax(serviceEndpoint, taxRequest);

            if (!taxResponse.Success)
            {
                kenticoLog.LogError("Tax estimation", $"Failed to estimate tax: {taxResponse.ErrorMessages}");
                return 0.0d;
            }

            return taxResponse.Payload;
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
 