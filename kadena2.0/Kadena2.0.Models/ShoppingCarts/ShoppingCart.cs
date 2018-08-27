using Kadena.Models.Product;
using System.Collections.Generic;

namespace Kadena.Models.ShoppingCarts
{
    public class ShoppingCart
    {
        public List<ShoppingCartItem> Items { get; set; }

        public int AddressId { get; set; }

        public int DistributorId { get; set; }

        public int UserId { get; set; }

        public int CustomerId { get; set; }

        public int CampaignId { get; set; }

        public int ProgramId { get; set; }

        public int ShippingOptionId { get; set; }

        public CampaignProductType Type { get; set; }

        public decimal TotalTax { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal PricedItemsTax { get; set; }
    }
}
