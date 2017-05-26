using System.Collections.Generic;

namespace Kadena.Dto.Checkout
{
    public class DeliveryMethodContainerDTO : ContainerDTO
    {
        public List<DeliveryMethodDTO> items { get; set; }
    }
}
