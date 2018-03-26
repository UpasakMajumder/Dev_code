using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.SubmitOrder.MicroserviceRequests
{
    public class AddressDTO
    {
        public int? KenticoAddressID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Address field is a mandatory field")]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "City is a mandatory field")]
        public string City { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "State is a mandatory field")]
        public string State { get; set; }

        public string StateDisplayName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Zip Code is a mandatory field")]
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int KenticoCountryID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Country is a mandatory field")]
        public string Country { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Country ISO code is a mandatory field")]
        public string isoCountryCode { get; set; }
        public int? KenticoStateID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Person name for delivery to is a mandatory field")]
        public string AddressPersonalName { get; set; }

        public string AddressCompanyName { get; set; }
    }
}
