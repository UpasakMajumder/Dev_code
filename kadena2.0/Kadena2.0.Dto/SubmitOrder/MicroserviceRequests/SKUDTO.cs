using Kadena.Dto.EstimateDeliveryPrice.MicroserviceRequests;
using System.ComponentModel.DataAnnotations;

namespace Kadena.Dto.SubmitOrder.MicroserviceRequests
{
    public class SKUDTO
    {
        public int KenticoSKUID { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Item Number in order's items is a mandatory field")]
        public string SKUNumber { get; set; }
        public string Name { get; set; }
        public bool HiResPdfAllowed { get; set; }
        public WeightDto Weight { get; set; }
  }
}
