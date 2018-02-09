using System.Collections.Generic;
using System.Linq;

namespace Kadena.Models.Checkout
{
    public class DeliveryCarriers
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public List<DeliveryCarrier> Items { get; set; }

        public DeliveryCarriers()
        {
            Items = new List<DeliveryCarrier>();
        }

        public void CheckMethod(int id)
        {
            Items.ForEach(i => i.CheckMethod(id));
        }

        public int GetDefaultMethodId()
        {
            foreach (var i in Items)
            {
                var defaultId = i.GetDefaultMethodId();
                if (defaultId != 0)
                    return defaultId;
            }

            return 0;            
        }

        public void UpdateSummaryText(string priceFrom, string price, string cannotBeDelivered, string customerPrice)
        {
            Items.ForEach(i => i.UpdateSummaryText(priceFrom, price, cannotBeDelivered, customerPrice));
        }

        public bool IsDisabled(int shippingMethod)
        {
            foreach (var i in Items)
            {
                if (i.IsDisabled(shippingMethod))
                    return true;
            }

            return false;
        }

        public bool IsPresent(int shippingMethod)
        {
            foreach (var i in Items)
            {
                if (i.IsPresent(shippingMethod))
                    return true;
            }

            return false;
        }

        public void RemoveCarriersWithoutOptions()
        {
            var toRemove = Items.Where(i => (i.items?.Count ?? 0)== 0).ToList();
            toRemove.ForEach(i => Items.Remove(i));
        }

        public void HidePrices()
        {
            foreach (DeliveryCarrier carrier in Items)
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

        public int CheckCurrentOrDefaultShipping(int currentShipping)
        {
            if (IsPresent(currentShipping) && !IsDisabled(currentShipping))
            {
                CheckMethod(currentShipping);
                return currentShipping;
            }
            else
            {
                int defaultMethodId = GetDefaultMethodId();
                CheckMethod(defaultMethodId);
                return defaultMethodId;
            }
        }
    }
}