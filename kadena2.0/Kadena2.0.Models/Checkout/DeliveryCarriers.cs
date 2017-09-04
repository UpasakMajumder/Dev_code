using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.Models.Checkout
{
    public class DeliveryCarriers
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public List<DeliveryCarrier> items { get; set; }

        public DeliveryCarriers()
        {
            items = new List<DeliveryCarrier>();
        }

        public void CheckMethod(int id)
        {
            items.ForEach(i => i.CheckMethod(id));
        }

        public int GetDefaultMethodId()
        {
            foreach (var i in items)
            {
                var defaultId = i.GetDefaultMethodId();
                if (defaultId != 0)
                    return defaultId;
            }

            return 0;            
        }

        public void UpdateSummaryText(string priceFrom, string price, string cannotBeDelivered, string customerPrice)
        {
            items.ForEach(i => i.UpdateSummaryText(priceFrom, price, cannotBeDelivered, customerPrice));
        }

        public bool IsDisabled(int shippingMethod)
        {
            foreach (var i in items)
            {
                if (i.IsDisabled(shippingMethod))
                    return true;
            }

            return false;
        }

        public bool IsPresent(int shippingMethod)
        {
            foreach (var i in items)
            {
                if (i.IsPresent(shippingMethod))
                    return true;
            }

            return false;
        }

        public void RemoveCarriersWithoutOptions()
        {
            var toRemove = items.Where(i => (i.items?.Count ?? 0)== 0).ToList();
            toRemove.ForEach(i => items.Remove(i));
        }

        public void HidePrices()
        {
            foreach (DeliveryCarrier carrier in items)
            {
                carrier.PricePrefix = string.Empty;
                carrier.Price = string.Empty;

                carrier.items.ForEach(option =>
                {
                    option.Price = string.Empty;
                    option.PricePrefix = string.Empty;
                });
            }

           
        }
    }
}