namespace Kadena2.MicroserviceClients.Responses
{
    public class TaxCalculatorServiceResponseDto
    {
        public bool Success { get; set; }
        public double Payload { get; set; }
        public string ErrorMessages { get; set; }
    }
}
