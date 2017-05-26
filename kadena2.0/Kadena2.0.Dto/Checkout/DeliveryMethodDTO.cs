using System.Collections.Generic;

namespace Kadena.Dto.Checkout
{
    public class DeliveryMethodDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public bool Opened { get; set; }
        public bool Disabled { get; set; }
        public string PricePrefix { get; set; }
        public string Price { get; set; }
        public string DatePrefix { get; set; }
        public string Date { get; set; }
        public List<DeliveryMethodTypeDTO> items { get; set; }
    }
}
