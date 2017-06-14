namespace Kadena2.MicroserviceClients.Responses
{
    public class SubmitOrderServiceResponseDto
    {
        public string Payload { get; set; }
        public bool Success { get; set; }
        public SubmitOrderErrorDto Error { get; set; }
    }
}
