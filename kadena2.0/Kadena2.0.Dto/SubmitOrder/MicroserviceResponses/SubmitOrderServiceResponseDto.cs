namespace Kadena2.MicroserviceClients.MicroserviceResponses
{
    public class SubmitOrderServiceResponseDto
    {
        public string Payload { get; set; }
        public bool Success { get; set; }
        public SubmitOrderErrorDto Error { get; set; }
    }
}
