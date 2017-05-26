using System.Collections.Generic;

namespace Kadena.Dto.Checkout
{
    public class DeliveryAddressesContainerDTO : ContainerDTO
    {
        public string AddAddressLabel { get; set; }

        public List<DeliveryAddressDTO> items { get; set; }
    }
}
