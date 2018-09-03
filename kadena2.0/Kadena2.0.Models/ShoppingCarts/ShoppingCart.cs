using Kadena.Models.Checkout;
using Kadena.Models.Product;
using System.Collections.Generic;

namespace Kadena.Models.ShoppingCarts
{
    public class ShoppingCart
    {
        public List<CartItemEntity> Items { get; set; }

        public DeliveryAddress Address { get; set; }

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

        public decimal TotalItemsWeight { get; set; }

        public decimal ShippingPrice { get; set; }
    }
}
