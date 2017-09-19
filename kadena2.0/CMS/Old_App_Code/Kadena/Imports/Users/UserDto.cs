using System.ComponentModel.DataAnnotations;

namespace Kadena.Old_App_Code.Kadena.Imports.Users
{
    public class UserDto
    {
        [Header(0, "Company")]
        [MaxLength(30)]
        public string Company { get; set; }

        [Header(1, "Organization ID")]
        [MaxLength(30)]
        public string OrganizationID { get; set; }

        [Header(2, "Tax Registration ID")]
        [MaxLength(30)]
        public string TaxRegistrationID { get; set; }

        [Header(3, "First name")]
        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Header(4, "Last name")]
        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }

        [Header(5, "Email")]
        [Required]
        [MaxLength(250)]
        public string Email { get; set; }

        [Header(6, "Contact name")]
        [MaxLength(30)]
        public string ContactName { get; set; }

        [Header(7, "Address line")]
        [MaxLength(50)]
        public string AddressLine { get; set; }

        [Header(8, "Address line 2")]
        [MaxLength(50)]
        public string AddressLine2 { get; set; }

        [Header(9, "City")]
        [MaxLength(30)]
        public string City { get; set; }

        [Header(10, "Postal code")]
        [MaxLength(30)]
        public string PostalCode { get; set; }

        [Header(11, "Country")]
        [MaxLength(30)]
        public string Country { get; set; }

        [Header(12, "Phone number")]
        [MaxLength(30)]
        public string PhoneNumber { get; set; }

        [Header(1000, "Role")] // role column should be last
        [Required]
        public string Role { get; set; }
    }
}