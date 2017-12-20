using CMS.Ecommerce;
using Kadena.BusinessLogic.Contracts;
using System;

namespace Kadena.BusinessLogic.Services
{
    public class DistributorService : IDistributorService
    {
        private readonly IDistributorService _kenticoDistributor;

        public DistributorService(IDistributorService kenticoDistributor)
        {
            if (kenticoDistributor == null)
            {
                throw new ArgumentNullException(nameof(kenticoDistributor));
            }
            this._kenticoDistributor = kenticoDistributor;
        }

        public string UpdateItemQuantity(int cartItemID, int quantity)
        {
            if (cartItemID != default(int) && quantity != default(int))
            {
                var shoppingCartItem = ShoppingCartItemInfoProvider.GetShoppingCartItemInfo(cartItemID);
                if (shoppingCartItem != null)
                {
                    shoppingCartItem.CartItemUnits = quantity;
                    shoppingCartItem.Update();
                    return "success";
                }
            }
            return "fail";
        }
    }
}
