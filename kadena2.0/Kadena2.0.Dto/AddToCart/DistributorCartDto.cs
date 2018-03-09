using System.Collections.Generic;

namespace Kadena.Dto.AddToCart
{
    public class DistributorCartDto
    {
        public int SKUID { get; set; }
        public int CartType { get; set; }
        public int AllocatedQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public List<DistributorCartItemDto> Items { get; set; }
    }
}
