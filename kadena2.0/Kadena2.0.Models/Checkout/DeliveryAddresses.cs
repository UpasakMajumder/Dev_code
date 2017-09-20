using System.Collections.Generic;
using System.Linq;

namespace Kadena.Models.Checkout
{
    public class DeliveryAddresses
    {
        public bool IsDeliverable { get; set; }
        public bool AvailableToAdd { get; set; }
        public string UnDeliverableText { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public NewAddressButton NewAddress { get; set; }

        public List<DeliveryAddress> items { get; set; }

        public string EmptyMessage { get; set; }

        public AddressDialog DialogUI { get; set; }

        public void CheckAddress(int id)
        {
            items.ForEach(i => i.Checked = false);

            var address = items.FirstOrDefault(i => i.Id == id);

            if (address != null)
            {
                address.Checked = true;
            }
        }

        public int GetDefaultAddressId()
        {
            if (items.Count == 0)
                return 0;

            return items[0].Id;
        }
    }
}