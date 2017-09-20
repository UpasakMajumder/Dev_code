namespace Kadena2.MicroserviceClients.MicroserviceResponses
{
    public class SubmitOrderServiceResponseDto // TODO refactor to common microservice response
    {
        public string Payload { get; set; }
        public bool Success { get; set; }
        public SubmitOrderErrorDto Error { get; set; }
    }
}
