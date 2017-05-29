using System.Collections.Generic;

namespace Kadena.WebAPI.Models
{
    public class DeliveryAddresses
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public string AddAddressLabel { get; set; }

        public List<DeliveryAddress> items { get; set; }
    }
}