using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.SubmitOrder
{
    public class ShippingOptionDTO
    {
        public int KenticoShippingOptionID { get; set; }

        public string ShippingService { get; set; }

        public string ShippingCompany { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Carrier Code is a mandatory field")]
        public string CarrierCode { get; set; }
    }
}
