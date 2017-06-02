using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.SubmitOrder
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
        [Required(AllowEmptyStrings = false, ErrorMessage = "Zip Code is a mandatory field")]
        public string Zip { get; set; }
        
        //this field is commented becouse phone number will be
        //received from Customer class
        //public string Phone { get; set; }
        public int KenticoCountryID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Country is a mandatory field")]
        public string Country { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Country ISO code is a mandatory field")]
        public string isoCountryCode { get; set; }
        public int? KenticoStateID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Person name for delivery to is a mandatory field")]
        /// <summary>
        /// personalized name of the address
        /// </summary>
        public string AddressPersonalName { get; set; }
        /// <summary>
        /// GET;SET; Company name to delivery
        /// 
        /// it's added 25/4/2017
        /// </summary>
        public string AddressCompanyName { get; set; }
    }
}
