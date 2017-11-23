namespace Kadena.Dto.CreditCard.MicroserviceResponses
{
    public class AuthorizeAmountResponseDto
    {
        public string AuthCode { get; set; }
        public object TransactionType { get; set; }
        public decimal TotalAmount { get; set; }
        public string TransactionKey { get; set; }
        public string ResponseStatus { get; set; }
        public bool Succeeded { get; set; }
        public string Warnings { get; set; }
        public string SummeryStatus { get; set; }
    }
}
