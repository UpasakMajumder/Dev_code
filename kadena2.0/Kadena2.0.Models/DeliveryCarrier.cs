using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.Models
{
    public class DeliveryCarrier
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public bool Opened { get; set; }
        public bool Disabled { get; set; }
        public string PricePrefix { get; set; }        
        public string Price { get; set; }
        public string DatePrefix { get; set; }
        public string Date { get; set; }
        public List<DeliveryOption> items { get; set; }
        public string Name { get; set; }

        public void SetShippingOptions(IEnumerable<DeliveryOption> services)
        {
            items = services.Where(s => s.CarrierId == this.Id).ToList();
        }

        public void UncheckAll()
        {
            this.Opened = false;
            items.ForEach(i => i.Checked = false);
        }

        public void CheckMethod(int id)
        {
            UncheckAll();

            var checkedItem = items.Where(i => i.Id == id).FirstOrDefault();
            if (checkedItem != null)
            {
                this.Opened = true;
                checkedItem.Checked = true;
            }
        }

        public bool IsDisabled(int shippingMethod)
        {
            return items.Where(i => i.Id == shippingMethod && i.Disabled).Any();
        }

        internal bool IsPresent(int shippingMethod)
        {
            return items.Where(i => i.Id == shippingMethod).Any();
        }

        public int GetDefaultMethodId()
        {
            var firstAvailable = items.Where(i => !i.Disabled).OrderBy(i => i.PriceAmount).FirstOrDefault();
            return firstAvailable?.Id ?? 0;
        }

        public void UpdateSummaryText(string priceFrom, string price, string cannotBeDelivered, string customerPrice)
        {
            items.ForEach(i => i.UpdateSummaryText(price, cannotBeDelivered, customerPrice));

            if (items.All(i => i.IsCustomerPrice))
            {
                this.PricePrefix = customerPrice;
                this.Price = string.Empty;
            }
            else
            {
                var cheapestItem = items.Where(i => !i.Disabled).OrderBy(i => i.PriceAmount).FirstOrDefault();

                if (cheapestItem != null)
                {
                    this.PricePrefix = priceFrom;
                    this.Price = cheapestItem.Price;
                }
                else
                {
                    this.PricePrefix = cannotBeDelivered;
                    this.Price = string.Empty;
                }
            }
        }

        public override string ToString()
        {
            return $"Carrier {Title} Id {Id}. Shipping options:{Environment.NewLine}{string.Join(Environment.NewLine, items)}";
        }
    }
}