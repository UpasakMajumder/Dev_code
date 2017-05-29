using System.Collections.Generic;

namespace Kadena.Dto.Checkout
{
    public class DeliveryAddressesDTO 
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public string AddAddressLabel { get; set; }

        public List<DeliveryAddressDTO> items { get; set; }
    }
}
