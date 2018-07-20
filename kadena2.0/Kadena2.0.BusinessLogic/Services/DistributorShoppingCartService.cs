using System;
using System.Linq;
using Kadena.BusinessLogic.Contracts;
using Kadena.Models;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena.Models.Product;
using System.Collections.Generic;
using Kadena.Models.AddToCart;
using Kadena.Models.CustomerData;
using Kadena.Models.ShoppingCarts;

namespace Kadena.BusinessLogic.Services
{
    public class DistributorShoppingCartService : IDistributorShoppingCartService
    {
        private readonly IKenticoUserProvider kenticoUsers;
        private readonly IKenticoResourceService resources;
        private readonly IShoppingCartProvider shoppingCart;
        private readonly IKenticoAddressBookProvider addressBookProvider;
        private readonly IKenticoProductsProvider productsProvider;
        private readonly IProductsService productsService;
        private readonly IKenticoBusinessUnitsProvider businessUnitsProvider;
        private readonly IKenticoSkuProvider skus;

        public DistributorShoppingCartService(IKenticoUserProvider kenticoUsers,
                                              IKenticoResourceService resources,
                                              IShoppingCartProvider shoppingCart,
                                              IKenticoAddressBookProvider addressBookProvider,
                                              IKenticoProductsProvider productsProvider,
                                              IProductsService productsService,
                                              IKenticoBusinessUnitsProvider businessUnitsProvider,
                                              IKenticoSkuProvider skus)
        {
            this.kenticoUsers = kenticoUsers ?? throw new ArgumentNullException(nameof(kenticoUsers));
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.shoppingCart = shoppingCart ?? throw new ArgumentNullException(nameof(shoppingCart));
            this.addressBookProvider = addressBookProvider ?? throw new ArgumentNullException(nameof(addressBookProvider));
            this.productsProvider = productsProvider ?? throw new ArgumentNullException(nameof(productsProvider));
            this.productsService = productsService ?? throw new ArgumentNullException(nameof(productsService));
            this.businessUnitsProvider = businessUnitsProvider ?? throw new ArgumentNullException(nameof(businessUnitsProvider));
            this.skus = skus ?? throw new ArgumentNullException(nameof(skus));
        }

        public DistributorCart GetCartDistributorData(int skuID, ShoppingCartTypes cartType = ShoppingCartTypes.GeneralInventory)
        {
            var userId = kenticoUsers.GetCurrentUser().UserId;

            ValidateBusinessUnits(userId);
            ValidateSku(skuID, cartType);
            int availableQty = GetInventoryAvailableQuantity(skuID, cartType);
            if (availableQty == 0)
            {
                throw new Exception(resources.GetResourceString("Kadena.AddToCart.NoStockAvailableError"));
            }
            int allocatedQty = GetAllocatedQuantity(skuID, cartType, userId);
            if (allocatedQty == 0)
            {
                throw new Exception(resources.GetResourceString("KDA.Cart.Update.ProductNotAllocatedMessage"));
            }
            return new DistributorCart()
            {
                SKUID = skuID,
                CartType = cartType,
                AvailableQuantity = availableQty,
                AllocatedQuantity = allocatedQty,
                Items = GetDistributorCartItems(skuID, userId, cartType)
            };
        }

        private void ValidateBusinessUnits(int userId)
        {
            int businessUnitsCount = businessUnitsProvider.GetUserBusinessUnits(userId)?.Count ?? 0;
            if (businessUnitsCount == 0)
            {
                throw new Exception(resources.GetResourceString("Kadena.AddToCart.BusinessUnitError"));
            }
        }

        private void ValidateSku(int skuID, ShoppingCartTypes cartType)
        {
            if (cartType == ShoppingCartTypes.GeneralInventory && !productsService.ProductHasValidSKUNumber(skuID))
            {
                throw new Exception(resources.GetResourceString("KDA.Cart.InvalidProduct"));
            }
        }

        private int GetAllocatedQuantity(int skuID, ShoppingCartTypes cartType, int userId)
        {
            int allocatedQty = cartType == ShoppingCartTypes.GeneralInventory
                ? shoppingCart.GetAllocatedQuantity(skuID, userId)
                : -1;
            return allocatedQty;
        }

        private int GetInventoryAvailableQuantity(int skuID, ShoppingCartTypes cartType)
        {
            int availableQty = cartType == ShoppingCartTypes.GeneralInventory
                ? skus.GetSkuAvailableQty(skuID)
                : -1;
            return availableQty;
        }

        // items - order item relation (skuid -> quantity), 
        // userid - creator of order, addressid - distributors address id
        public IEnumerable<DistributorCart> CreateCart(Dictionary<int, int> items, int userId, int addressId)
        {
            var inventoryType = ShoppingCartTypes.GeneralInventory;
            return items.Select(i =>
            {
                int availableQty = GetInventoryAvailableQuantity(i.Key, inventoryType);
                int allocatedQty = GetAllocatedQuantity(i.Key, inventoryType, userId);
                return new DistributorCart
                {
                    CartType = inventoryType,
                    SKUID = i.Key,
                    Items = new List<DistributorCartItem>
                    {
                        CreateDistributorCartItem(0, addressId, i.Value)
                    }
                };
            });
        }

        private List<DistributorCartItem> GetDistributorCartItems(int skuID, int userId, ShoppingCartTypes cartType = ShoppingCartTypes.GeneralInventory)
        {
            CampaignsProduct product = productsProvider.GetCampaignProduct(skuID) ?? throw new Exception("Invalid product");

            List<AddressData> distributors = addressBookProvider.GetAddressesListByUserID(userId, (int)cartType, product.CampaignID);
            return distributors.Select(x =>
            {
                return CreateDistributorCartItem(x.DistributorShoppingCartID, x.AddressID, shoppingCart.GetItemQuantity(skuID, x.DistributorShoppingCartID));
            }).ToList();
        }

        private DistributorCartItem CreateDistributorCartItem(int cartId, int addressId, int quantity)
        {
            return new DistributorCartItem()
            {
                DistributorID = addressId,
                ShoppingCartID = cartId,
                Quantity = quantity
            };
        }

        public int UpdateDistributorCarts(DistributorCart cartDistributorData, int userId = 0)
        {
            if (userId < 1)
            {
                userId = kenticoUsers.GetCurrentUser().UserId;
            }

            if ((cartDistributorData?.Items.Count ?? 0) <= 0)
            {
                throw new Exception("Invalid request");
            }
            CampaignsProduct product = productsProvider.GetCampaignProduct(cartDistributorData.SKUID) ?? throw new Exception("Invalid product");

            cartDistributorData.Items
                .Where(i => i.ShoppingCartID.Equals(default(int)) && i.Quantity > 0)
                ?.ToList()
                .ForEach(x => CreateDistributorCart(x, product, userId, cartDistributorData.CartType));

            cartDistributorData.Items
                .Where(i => i.ShoppingCartID > 0 && i.Quantity > 0)
                ?.ToList()
                .ForEach(x => shoppingCart.UpdateDistributorCart(x, product, cartDistributorData.CartType));

            cartDistributorData.Items
                .Where(i => i.ShoppingCartID > 0 && i.Quantity == 0)
                ?.ToList()
                .ForEach(x => shoppingCart.DeleteDistributorCartItem(x.ShoppingCartID, cartDistributorData.SKUID));

            return shoppingCart.GetDistributorCartCount(userId, product.CampaignID, cartDistributorData.CartType);
        }

        private void CreateDistributorCart(DistributorCartItem distributorCartItem, CampaignsProduct product, int userId,
            ShoppingCartTypes inventoryType = ShoppingCartTypes.GeneralInventory)
        {
            int cartID = shoppingCart.CreateDistributorCart(distributorCartItem.DistributorID,
                product.CampaignID, product.ProgramID,
                userId, inventoryType);
            shoppingCart.AddDistributorCartItem(cartID, distributorCartItem, product, inventoryType);
        }

        public string UpdateCartQuantity(Distributor submitRequest)
        {
            // TODO : move logic from provider to here
            return shoppingCart.UpdateCartQuantity(submitRequest);
        }
    }
}
