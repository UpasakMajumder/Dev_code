namespace Kadena.Dto.EstimateDeliveryPrice.MicroserviceResponses
{
    public class EstimateDeliveryPriceResponseDto
    {
        public bool Success { get; set; }
        public EstimateDeliveryPricePayloadDto payload { get; set; }
        public string ErrorMessages { get; set; }
    }
}
