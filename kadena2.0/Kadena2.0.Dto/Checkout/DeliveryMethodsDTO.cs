using System.Collections.Generic;

namespace Kadena.Dto.Checkout
{
    public class DeliveryMethodsDTO 
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public List<DeliveryMethodDTO> items { get; set; }
    }
}
