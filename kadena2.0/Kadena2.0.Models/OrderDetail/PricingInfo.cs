using System;
using System.Collections.Generic;

namespace Kadena.Models.OrderDetail
{
    public class PricingInfo
    {
        public string Title { get; set; }
        public IList<PricingInfoItem> Items { get; set; }
    }
}