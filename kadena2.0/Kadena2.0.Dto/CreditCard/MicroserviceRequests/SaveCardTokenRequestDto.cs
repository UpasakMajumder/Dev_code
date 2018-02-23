namespace Kadena.Dto.CreditCard.MicroserviceRequests
{
    public class SaveCardTokenRequestDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string CardNumber { get; set; }
        public string CardExpirationDate { get; set; }
        public string Token { get; set; }
        public bool SaveToken { get; set; }
    }
}
