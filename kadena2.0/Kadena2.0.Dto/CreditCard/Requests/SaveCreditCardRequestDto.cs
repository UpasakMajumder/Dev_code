using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.CreditCard.Requests
{
    public class SaveCreditCardRequestDto
    {
        [Required]
        public bool Save { get; set; }
        public string Name { get; set; }
        public string CardNumber { get; set; }
        public string SubmissionID { get; set; }
    }
}
