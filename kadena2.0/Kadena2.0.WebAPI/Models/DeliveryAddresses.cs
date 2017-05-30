using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.Models
{
    public class DeliveryAddresses
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public string AddAddressLabel { get; set; }

        public List<DeliveryAddress> items { get; set; }

        public void CheckAddress(int id)
        {
            items.ForEach(i => i.Checked = false);

            var address = items.FirstOrDefault(i => i.Id == id);

            if (address != null)
            {
                address.Checked = true;
            }
        }
    }
}