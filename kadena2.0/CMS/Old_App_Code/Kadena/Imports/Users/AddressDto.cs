using System.ComponentModel.DataAnnotations;

namespace Kadena.Old_App_Code.Kadena.Imports.Users
{
    public class AddressDto
    {
        [Header(0, "Contact name")]
        [MaxLength(30)]
        [Required]
        public string ContactName { get; set; }

        [Header(1, "Address line")]
        [MaxLength(50)]
        [Required]
        public string AddressLine { get; set; }

        [Header(2, "Address line 2")]
        [MaxLength(50)]
        public string AddressLine2 { get; set; }

        [Header(3, "City")]
        [MaxLength(30)]
        [Required]
        public string City { get; set; }

        [Header(4, "Postal code")]
        [MaxLength(30)]
        [Required]
        public string PostalCode { get; set; }

        [Header(5, "Country")]
        [MaxLength(30)]
        [Required]
        public string Country { get; set; }

        [Header(6, "State")]
        [MaxLength(30)]
        public string State { get; set; }

        [Header(7, "Phone number")]
        [MaxLength(30)]
        public string PhoneNumber { get; set; }
    }
}