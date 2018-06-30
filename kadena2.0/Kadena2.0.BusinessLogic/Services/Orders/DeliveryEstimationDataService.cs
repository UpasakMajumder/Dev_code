using Kadena.BusinessLogic.Contracts.Orders;
using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;
using Kadena.Models.Shipping;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.MicroserviceClients.Contracts;
using System;
using System.Linq;

namespace Kadena.BusinessLogic.Services.Orders
{
    public class DeliveryEstimationDataService : IDeliveryEstimationDataService
    {
        private readonly IKenticoResourceService resources;
        private readonly IShippingCostServiceClient shippingCosts;
        private readonly IKenticoLogger log;

        public DeliveryEstimationDataService(IKenticoResourceService resources, IShippingCostServiceClient shippingCosts, IKenticoLogger log)
        {
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.shippingCosts = shippingCosts ?? throw new ArgumentNullException(nameof(shippingCosts));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public AddressDto GetSourceAddress()
        {
            var addressLines = new[]
            {
                resources.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderAddressLine1"),
                resources.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderAddressLine2")
            }.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();

            return new AddressDto()
            {
                City = resources.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderCity"),
                Country = resources.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderCountry"),
                Postal = resources.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderPostal"),
                State = resources.GetSiteSettingsKey("KDA_EstimateDeliveryPrice_SenderState"),
                StreetLines = addressLines
            };

        }

        public EstimateDeliveryPriceRequestDto[] GetDeliveryEstimationRequestData(string provider, string service, decimal weight, AddressDto target)
        {
            return new[]
            {
                new EstimateDeliveryPriceRequestDto
                {
                    Provider = provider,
                    ProviderService = service.Replace("#", ""), // hack to solve non-unique service keys,
                    SourceAddress = GetSourceAddress(),
                    TargetAddress = target,
                    Weight = new WeightDto
                    {
                        Value = weight,
                        Unit = resources.GetMassUnit()
                    }
                }
            };
        }

        public decimal GetShippingCost(string provider, string service, decimal weight, AddressDto target)
        {
            if (ShippingOption.Ground.ToLower().Equals(service.ToLower()))
            {
                return decimal.Zero;
            }

            var shippingCostRequest = GetDeliveryEstimationRequestData(provider, service,
                weight, target);

            var response = shippingCosts.EstimateShippingCost(shippingCostRequest).Result;

            if (response.Success == false || response.Payload.Length < 1 || !response.Payload[0].Success)
            {
                log.LogError(this.GetType().Name, $"Call from '{service}' provider resulted with error {response.ErrorMessages}");
                throw new InvalidOperationException(response.ErrorMessages);
            }

            return response.Payload[0].Cost;
        }
    }
}
