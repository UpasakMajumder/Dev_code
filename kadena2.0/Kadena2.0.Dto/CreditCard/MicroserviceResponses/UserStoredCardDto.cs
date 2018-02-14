using System;

namespace Kadena.Dto.CreditCard.MicroserviceResponses
{
    public class UserStoredCardDto
    {
        public string UserId { get; set; }
        public string Id { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string Name { get; set; }
        public string CardNumber { get; set; }
        public DateTime TokenExpirationDate { get; set; }
        public string Token { get; set; }
    }
}
