namespace Kadena.Dto.CreditCard.MicroserviceRequests
{
    public class AuthorizeAmountRequestDto
    {
        public UserDto User { get; set; }
        public PaymentDataDto PaymentData { get; set; }
        public AdditionalAmountsDto AdditionalAmounts { get; set; }
        public SapDetailsDto SapDetails { get; set; }
        public int Currency { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
