namespace Kadena.Dto.CreditCard.MicroserviceRequests
{
    public class SaveCardTokenRequestDto
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Optional, last 4 digits
        /// </summary>
        public string CardNumber { get; set; }
        public string TokenExpirationDate { get; set; }
        public string Token { get; set; }
    }
}
