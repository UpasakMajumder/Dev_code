using System.Collections.Generic;

namespace Kadena.Dto.Checkout
{
    public class PaymentMethodsDTO
    {
        public bool IsPayable { get; set; }
        public string UnPayableText { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<PaymentMethodDTO> Items { get; set; }
    }
}
