using Kadena.Models.ShoppingCarts;
using System.Collections.Generic;

namespace Kadena.Models.AddToCart
{
    public class DistributorCart
    {
        public int SKUID { get; set; }
        public ShoppingCartTypes CartType { get; set; }
        public int AllocatedQuantity { get; set; }
        public int AvailableQuantity { get; set; }
        public List<DistributorCartItem> Items { get; set; }
    }
}
