using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.Models
{
    public class DeliveryMethod
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
        public List<DeliveryService> items { get; set; }

        public void SetShippingOptions(IEnumerable<DeliveryService> services)
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

        public int GetDefaultMethodId()
        {
            if (items.Count < 1)
                return 0;

            return items[0].Id;
        }

        public void UpdateSummaryText(string priceFrom, string cannotBeDelivered)
        {
            items.ForEach(i => i.UpdateSummaryText(priceFrom, cannotBeDelivered));

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
}