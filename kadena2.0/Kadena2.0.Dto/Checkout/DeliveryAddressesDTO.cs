using System.Collections.Generic;

namespace Kadena.Dto.Checkout
{
    public class DeliveryAddressesDTO 
    {
        public bool IsDeliverable { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AddAddressLabel { get; set; }
        public List<DeliveryAddressDTO> Items { get; set; }
    }
}
