using System.Collections.Generic;

namespace Kadena.Dto.Checkout
{
    public class PaymentMethodContainerDTO : ContainerDTO
    {
        public List<PaymentMethodDTO> items { get; set; }
    }
}
