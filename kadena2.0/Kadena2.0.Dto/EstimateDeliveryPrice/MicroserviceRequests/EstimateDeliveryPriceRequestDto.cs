namespace Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests
{
    public class EstimateDeliveryPriceRequestDto
    {
        public AddressDto SourceAddress { get; set; }
        public AddressDto TargetAddress { get; set; }
        public WeightDto Weight { get; set; }
        public string Provider { get; set; }
        public string ProviderService { get; set; }
    }
}
