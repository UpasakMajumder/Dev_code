using System.Collections.Generic;

namespace Kadena.Dto.ViewOrder.Responses
{
    public class PricingInfoDTO
    {
        public string Title { get; set; }
        public IList<PricingInfoItemDTO> Items { get; set; }
    }
}