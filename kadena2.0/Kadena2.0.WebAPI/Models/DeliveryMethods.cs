using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.Models
{
    public class DeliveryMethods
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public List<DeliveryMethod> items { get; set; }

        public void CheckMethod(int id)
        {
            items.ForEach(i => i.CheckMethod(id));
        }

        public int GetDefaultMethodId()
        {
            if (items.Count == 0)
                return 0;

            return items[0].GetDefaultMethodId();
        }

        public void UpdateSummaryText(string priceFrom, string price, string cannotBeDelivered)
        {
            items.ForEach(i => i.UpdateSummaryText(priceFrom, price, cannotBeDelivered));
        }
    }
}