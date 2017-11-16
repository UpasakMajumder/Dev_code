namespace Kadena.Dto.CreditCard
{
    public class PaymentDataDto
    {
        public string CardTokenId { get; set; }
        public string Token { get; set; }
        public string PaymentProvider { get; set; }
        public string TransactionKey { get; set; }
    }
}
