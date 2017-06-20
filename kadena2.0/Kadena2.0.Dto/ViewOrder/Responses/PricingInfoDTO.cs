using System.Collections.Generic;

namespace Kadena.Dto.ViewOrder.Responses
{
    public class PricingInfoDTO
    {
        public string title { get; set; }
        public IList<PricingInfoItemDTO> items { get; set; }
    }
}