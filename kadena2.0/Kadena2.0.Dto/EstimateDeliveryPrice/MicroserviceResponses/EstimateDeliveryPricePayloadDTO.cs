namespace Kadena.Dto.EstimateDeliveryPrice.MicroserviceResponses
{
    public class EstimateDeliveryPricePayloadDto
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string ProviderName { get; set; }
        public string ProviderService { get; set; }
        public decimal Cost { get; set; }
        public string Currency { get; set; }
    }
}
