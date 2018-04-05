namespace Kadena.Models.TemplatedProduct
{
    public class EmailProofRequest
    {
        public string EmailProofUrl { get; set; }
        public string Message { get; set; }
        public string RecepientEmail { get; set; }
        public string SenderEmail { get; set; }
        public string Subject { get; set; }
    }
}
