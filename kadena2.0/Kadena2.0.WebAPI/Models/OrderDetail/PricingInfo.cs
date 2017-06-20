using System.Collections.Generic;

namespace Kadena.WebAPI.Models.OrderDetail
{
    public class PricingInfo
    {
        public string title { get; set; }
        public IList<PricingInfoItem> items { get; set; }
    }
}