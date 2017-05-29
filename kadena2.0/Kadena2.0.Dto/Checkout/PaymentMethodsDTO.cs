using System.Collections.Generic;

namespace Kadena.Dto.Checkout
{
    public class PaymentMethodsDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public List<PaymentMethodDTO> items { get; set; }
    }
}
