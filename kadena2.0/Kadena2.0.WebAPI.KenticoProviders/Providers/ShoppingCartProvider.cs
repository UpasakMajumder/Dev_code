using AutoMapper;
using CMS.DataEngine;
using CMS.CustomTables;
using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.CustomerData;
using Kadena.Models.Product;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using Kadena.Models.AddToCart;
using Kadena.Models.ShoppingCarts;
using Kadena.Models.SiteSettings;
using Kadena.Models.CampaignData;

namespace Kadena.WebAPI.KenticoProviders
{
    public class ShoppingCartProvider : IShoppingCartProvider
    {
        private readonly IKenticoResourceService resources;
        private readonly IMapper mapper;
        private readonly IShippingEstimationSettings estimationSettings;
        private readonly IKenticoProductsProvider productProvider;
        private readonly IKenticoLocalizationProvider localization;

        private readonly string campaignClassName = "KDA.CampaignsProduct";
        private readonly string CustomTableName = "KDA.UserAllocatedProducts";

        public ShoppingCartProvider(IKenticoResourceService resources, IMapper mapper, IShippingEstimationSettings estimationSettings, IKenticoProductsProvider productProvider
           , IKenticoLocalizationProvider localization)
        {
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.estimationSettings = estimationSettings ?? throw new ArgumentNullException(nameof(estimationSettings));
            this.productProvider = productProvider ?? throw new ArgumentNullException(nameof(productProvider));
            this.localization = localization ?? throw new ArgumentNullException(nameof(localization));
        }

        public DeliveryAddress GetCurrentCartShippingAddress()
        {
            var address = ECommerceContext.CurrentShoppingCart.ShoppingCartShippingAddress;
            var shippingAddress = mapper.Map<DeliveryAddress>(address);
            if (shippingAddress != null)
            {
                shippingAddress.Country = localization.GetCountries().FirstOrDefault(c => c.Id == shippingAddress.Country.Id);
                shippingAddress.State = localization.GetStates().FirstOrDefault(c => c.Id == shippingAddress.State.Id);
            }
            return shippingAddress;
        }

        public DeliveryAddress GetAddress(int addressId)
        {
            var address = AddressInfoProvider.GetAddressInfo(addressId);
            return mapper.Map<DeliveryAddress>(address);
        }

        public BillingAddress GetDefaultBillingAddress()
        {
            var streets = new[]
            {
                estimationSettings.SenderAddressLine1,
                estimationSettings.SenderAddressLine2,
            }.Where(i => !string.IsNullOrEmpty(i)).ToList();

            string countryName = estimationSettings.SenderCountry;
            string stateName = estimationSettings.SenderState;
            int countryId = CountryInfoProvider.GetCountryInfoByCode(countryName).CountryID;
            var state = localization.GetStates().FirstOrDefault(c => c.StateCode == stateName);

            return new BillingAddress()
            {
                Street = streets,
                City = estimationSettings.SenderCity,
                Country = countryName,
                CountryId = countryId,
                Zip = estimationSettings.SenderPostal,
                State = state
            };
        }

        public DeliveryCarrier[] GetShippingCarriers()
        {
            var shippingOptions = GetShippingOptions();
            var carriers = CarrierInfoProvider.GetCarriers(SiteContext.CurrentSiteID).ToArray();

            var deliveryMethods = mapper.Map<DeliveryCarrier[]>(carriers);

            foreach (DeliveryCarrier dm in deliveryMethods)
            {
                dm.SetShippingOptions(shippingOptions);
                dm.Icon = GetShippingProviderIcon(dm.Name);
                dm.Title = resources.ResolveMacroString(dm.Title);
            }

            return deliveryMethods;
        }

        /// <summary>
        /// Hardcoded until finding some convinient way to configure it in Kentico
        /// </summary>
        public string GetShippingProviderIcon(string name)
        {
            if (name != null)
            {
                var dictionary = new Dictionary<string, string>()
                {
                    {"fedex","fedex-delivery"},
                    {"usps","usps-delivery" },
                    {"ups","ups-delivery" }
                };

                foreach (var kvp in dictionary)
                {
                    if (name.ToLower().Contains(kvp.Key))
                        return kvp.Value;
                }
            }
            return string.Empty;
        }

        public DeliveryOption[] GetShippingOptions()
        {
            var services = ShippingOptionInfoProvider.GetShippingOptions(SiteContext.CurrentSiteID).Where(s => s.ShippingOptionEnabled).ToArray();
            var result = mapper.Map<DeliveryOption[]>(services);
            foreach (var item in result)
            {
                var carrier = CarrierInfoProvider.GetCarrierInfo(item.CarrierId);
                item.CarrierCode = carrier.CarrierName;
                item.Title = resources.ResolveMacroString(item.Title);
            }

            GetShippingPrice(result);
            return result;
        }

        public DeliveryOption GetShippingOption(int id)
        {
            var service = ShippingOptionInfoProvider.GetShippingOptionInfo(id);
            var result = mapper.Map<DeliveryOption>(service);
            var carrier = CarrierInfoProvider.GetCarrierInfo(service.ShippingOptionCarrierID);
            result.CarrierCode = carrier.CarrierName;
            return result;
        }

        private void GetShippingPrice(DeliveryOption[] services)
        {
            if (ECommerceContext.CurrentShoppingCart.ShoppingCartShippingAddress == null)
            {
                foreach (var s in services.Where(s => !s.IsCustomerPrice))
                {
                    s.Disable();
                }
                return;
            }

            // this method's approach comes from origial kentico webpart (ShippingSeletion)
            int originalCartShippingId = ECommerceContext.CurrentShoppingCart.ShoppingCartShippingOptionID;

            foreach (var s in services)
            {
                var cart = ECommerceContext.CurrentShoppingCart;
                cart.ShoppingCartShippingOptionID = s.Id;
                s.PriceAmount = cart.TotalShipping;
                s.Price = String.Format("$ {0:#,0.00}", ECommerceContext.CurrentShoppingCart.TotalShipping);

                if (cart.TotalShipping == 0.0d && !s.IsCustomerPrice)
                {
                    s.Disabled = true;
                }
            }

            ECommerceContext.CurrentShoppingCart.ShoppingCartShippingOptionID = originalCartShippingId;
        }

        public PaymentMethod[] GetPaymentMethods()
        {
            var paymentOptionInfoCollection = PaymentOptionInfoProvider.GetPaymentOptions(SiteContext.CurrentSiteID).Where(p => p.PaymentOptionEnabled).ToArray();
            var methods = mapper.Map<PaymentMethod[]>(paymentOptionInfoCollection);

            foreach (var method in methods)
            {
                method.Title = resources.ResolveMacroString(method.DisplayName);
            }
            return methods;
        }

        public PaymentMethod GetPaymentMethod(int id)
        {
            var paymentInfo = PaymentOptionInfoProvider.GetPaymentOptionInfo(id);
            var method = mapper.Map<PaymentMethod>(paymentInfo);
            method.Title = resources.ResolveMacroString(method.DisplayName);
            return method;
        }

        public DateTime? GetRequestedDeliveryDate()
        {
            var deliveryDate = ECommerceContext.CurrentShoppingCart.GetDateTimeValue("ShoppingCartRequestedDeliveryDate", DateTime.MinValue);
            DateTime? nullableDeliveryDate = null;

            if (deliveryDate != DateTime.MinValue)
            {
                nullableDeliveryDate = deliveryDate;
            }

            return nullableDeliveryDate;
        }

        public void SetRequestedDeliveryDate(DateTime? requestedDeliveryDate)
        {
            ECommerceContext.CurrentShoppingCart.SetValue("ShoppingCartRequestedDeliveryDate", requestedDeliveryDate);
        }

        public ShoppingCartTotals GetShoppingCartTotals()
        {
            return new ShoppingCartTotals()
            {
                TotalItemsPrice = (decimal)ECommerceContext.CurrentShoppingCart.TotalItemsPrice,
                TotalShipping = (decimal)ECommerceContext.CurrentShoppingCart.TotalShipping,
                TotalTax = (decimal)ECommerceContext.CurrentShoppingCart.TotalTax
            };
        }

        public void SetShoppingCartAddress(int addressId)
        {
            var cart = ECommerceContext.CurrentShoppingCart;

            if (cart.ShoppingCartShippingAddress == null || cart.ShoppingCartShippingAddress.AddressID != addressId)
            {
                var address = AddressInfoProvider.GetAddressInfo(addressId);
                cart.ShoppingCartShippingAddress = address;
                ShoppingCartInfoProvider.SetShoppingCartInfo(cart);
            }
        }

        public void SelectShipping(int shippingOptionId)
        {
            var cart = ECommerceContext.CurrentShoppingCart;

            if (cart.ShoppingCartShippingOptionID != shippingOptionId)
            {
                cart.ShoppingCartShippingOptionID = shippingOptionId;
                cart.SubmitChanges(true);
            }
        }

        public int GetCurrentCartShippingOptionId()
        {
            return ECommerceContext.CurrentShoppingCart.ShoppingCartShippingOptionID;
        }

        public int GetShoppingCartId(int userId, int siteId)
        {
            var siteName = SiteInfoProvider.GetSiteInfo(siteId)?.SiteName ?? string.Empty;

            if (string.IsNullOrEmpty(siteName))
            {
                return 0;
            }

            return ShoppingCartInfoProvider.GetShoppingCartInfo(userId, siteName)?.ShoppingCartID ?? 0;
        }

        private ShoppingCartInfo GetShoppingCartInternal(int shoppingCartId = 0)
        {
            return shoppingCartId > 0
                ? ShoppingCartInfoProvider.GetShoppingCartInfo(shoppingCartId)
                : ECommerceContext.CurrentShoppingCart;
        }

        public void ClearCart(int shoppingCartId = 0)
        {
            var shoppingCart = GetShoppingCartInternal(shoppingCartId);
            ShoppingCartInfoProvider.DeleteShoppingCartInfo(shoppingCart);
        }

        public double GetCurrentCartTotalItemsPrice()
        {
            return ECommerceContext.CurrentShoppingCart.TotalItemsPrice;
        }

        public double GetCurrentCartShippingCost()
        {
            return ECommerceContext.CurrentShoppingCart.Shipping;
        }

        public string UpdateCartQuantity(Distributor distributorData)
        {
            if (distributorData.ItemQuantity < 1)
            {
                throw new Exception(ResHelper.GetString("KDA.Cart.Update.MinimumQuantityError", LocalizationContext.CurrentCulture.CultureCode));
            }
            var shoppingCartItem = ShoppingCartItemInfoProvider.GetShoppingCartItemInfo(distributorData.CartItemId)
                ?? throw new Exception(ResHelper.GetString("KDA.Cart.Update.Failure", LocalizationContext.CurrentCulture.CultureCode));
            if (distributorData.InventoryType == 1)
            {
                var shoppingCartIDs = ShoppingCartInfoProvider.GetShoppingCarts().WhereEquals("ShoppingCartUserID", distributorData.UserID).WhereEquals("ShoppingCartInventoryType", 1).ToList().Select(x => x.ShoppingCartID).ToList();
                var shoppingcartItems = ShoppingCartItemInfoProvider.GetShoppingCartItems().WhereIn("ShoppingCartID", shoppingCartIDs).WhereEquals("SKUID", shoppingCartItem.SKUID).ToList();
                int totalItems = shoppingcartItems
                    .Where(i => i != null && i.CartItemID != distributorData.CartItemId)
                    .Sum(i => i.CartItemUnits);
                var sku = SKUInfoProvider.GetSKUInfo(shoppingCartItem.SKUID);
                var allocatedQuantity = productProvider.GetAllocatedProductQuantityForUser(sku.SKUID, distributorData.UserID);
                if (sku.SKUAvailableItems < totalItems + distributorData.ItemQuantity)
                {
                    throw new Exception(ResHelper.GetString("KDA.Cart.Update.InsufficientStockMessage", LocalizationContext.CurrentCulture.CultureCode));
                }
                if (allocatedQuantity > -1 && allocatedQuantity < totalItems + distributorData.ItemQuantity)
                {
                    throw new Exception(ResHelper.GetString("Kadena.AddToCart.AllocatedProductQuantityError", LocalizationContext.CurrentCulture.CultureCode));
                }
            }
            shoppingCartItem.CartItemUnits = distributorData.ItemQuantity;
            shoppingCartItem.Update();
            return ResHelper.GetString("KDA.Cart.Update.Success");
        }

        public List<int> GetUserIDsWithShoppingCart(int campaignID, int productType)
        {
            return ShoppingCartInfoProvider.GetShoppingCarts().WhereEquals("ShoppingCartCampaignID", campaignID)
                                                                     .WhereEquals("ShoppingCartInventoryType", productType).ToList().Select(x => x.ShoppingCartUserID).Distinct().ToList();
        }

        public bool IsCartContainsInvalidProduct(int shoppingCartId = 0)
        {
            bool isValidCart = true;
            ShoppingCartInfo shoppingCart = GetShoppingCartInternal(shoppingCartId);
            if (shoppingCart != null)
            {
                var inValidCartItems = shoppingCart.CartItems.Where(x => string.IsNullOrWhiteSpace(x.SKU.SKUNumber) || x.SKU.SKUNumber.Equals("00000"));
                if (inValidCartItems != null && inValidCartItems.Count() > 0)
                {
                    isValidCart = false;
                }
            }
            return isValidCart;
        }

        public List<int> GetCampaingShoppingCartIDs(int campaignID)
        {
            return ShoppingCartInfoProvider.GetShoppingCarts(SiteContext.CurrentSiteID).WhereEquals("ShoppingCartCampaignID", campaignID)?.ToList().Select(x => x.ShoppingCartID).ToList();
        }

        public List<int> GetUserShoppingCartIDs(int userID)
        {
            return ShoppingCartInfoProvider.GetShoppingCarts(SiteContext.CurrentSiteID)
                                           .WhereEquals("ShoppingCartUserID", userID)
                                           .And()
                                           .WhereEquals("ShoppingCartInventoryType", 1)
                                           ?.ToList().Select(x => x.ShoppingCartID).ToList();
        }

        public bool ValidateAllCarts(int userID = 0, int campaignID = 0)
        {
            bool isValid = true;
            List<int> shoppingCartIDs = new List<int>();
            if (campaignID > 0)
            {
                shoppingCartIDs = GetCampaingShoppingCartIDs(campaignID);
            }
            else if (userID > 0)
            {
                shoppingCartIDs = GetUserShoppingCartIDs(userID);
            }
            if (shoppingCartIDs != null && shoppingCartIDs.Count > 0)
            {
                foreach (int shoppingCartID in shoppingCartIDs)
                {
                    if (!IsCartContainsInvalidProduct(shoppingCartID))
                    {
                        isValid = false;
                        break;
                    }
                }
            }
            return isValid;
        }
        public ShoppingCartInfo GetShoppingCartByID(int cartID)
        {
            return ShoppingCartInfoProvider.GetShoppingCartInfo(cartID);
        }
        public void UpdateBusinessUnit(ShoppingCartInfo cart, long businessUnitID)
        {
            cart.SetValue("BusinessUnitIDForDistributor", businessUnitID);
            cart.Update();
        }

        public List<int> GetShoppingCartIDByInventoryType(CampaignProductType cartType, int userID, int campaignID = 0)
        {
            return ShoppingCartInfoProvider.GetShoppingCarts(SiteContext.CurrentSiteID)
                                    .OnSite(SiteContext.CurrentSiteID)
                                    .WhereEquals("ShoppingCartUserID", userID)
                                    .WhereEquals("ShoppingCartCampaignID", campaignID)
                                    .WhereEquals("ShoppingCartInventoryType", (int)cartType)
                                    ?.ToList().Select(x => x.ShoppingCartID).ToList();
        }

        public int GetPreBuyDemandCount(int SKUID)
        {
            return ShoppingCartItemInfoProvider.GetShoppingCartItems()
                                         .OnSite(SiteContext.CurrentSiteID)
                                         .Where(x => x.SKUID.Equals(SKUID))
                                         .Sum(x => x.CartItemUnits);
        }

        public int GetDistributorCartID(int distributorID, CampaignProductType cartType = CampaignProductType.GeneralInventory, int campaignID = 0)
        {
            ShoppingCartInfo cart = ShoppingCartInfoProvider.GetShoppingCarts(SiteContext.CurrentSiteID)
                                    .OnSite(SiteContext.CurrentSiteID)
                                    .WhereEquals("ShoppingCartDistributorID", distributorID)
                                    .And()
                                    .WhereEqualsOrNull("ShoppingCartCampaignID", campaignID)
                                    .And()
                                    .WhereEquals("ShoppingCartInventoryType", (int)cartType).FirstOrDefault();
            return cart != null ? cart.ShoppingCartID : 0;
        }

        public int GetItemQuantity(int SKUID, int shoppingCartID)
        {
            return ShoppingCartItemInfoProvider.GetShoppingCartItems()
                                                .OnSite(SiteContext.CurrentSiteID)
                                                .Where(x => x.ShoppingCartID.Equals(shoppingCartID) && x.SKUID.Equals(SKUID))
                                                .Sum(x => x.CartItemUnits);
        }

        public void RemoveCurrentItemsFromStock(int shoppingCartId = 0)
        {
            var shoppingCart = GetShoppingCartInternal(shoppingCartId);

            var items = shoppingCart.CartItems;

            foreach (var i in items)
            {
                if (i.GetValue("ProductType", string.Empty).Contains(ProductTypes.InventoryProduct))
                {
                    int toRemove = i.CartItemUnits <= i.SKU.SKUAvailableItems ? i.CartItemUnits : i.SKU.SKUAvailableItems;
                    i.SKU.SKUAvailableItems -= toRemove;
                    i.SKU.SubmitChanges(false);
                    i.SKU.MakeComplete(true);
                    i.SKU.Update();
                }
            }
        }
        public int CreateDistributorCart(int distributorId, int campaignId, int programId, int userID, CampaignProductType cartType = CampaignProductType.GeneralInventory)
        {
            ShippingOptionInfo shippingOption = ShippingOptionInfoProvider.GetShippingOptionInfo(resources.GetSiteSettingsKey(Settings.KDA_DefaultShipppingOption), SiteContext.CurrentSiteName);
            var customerAddress = AddressInfoProvider.GetAddressInfo(distributorId);
            ShoppingCartInfo cart = new ShoppingCartInfo()
            {
                ShoppingCartSiteID = SiteContext.CurrentSiteID,
                ShoppingCartCustomerID = distributorId,
                ShoppingCartCurrencyID = CurrencyInfoProvider.GetMainCurrency(SiteContext.CurrentSiteID).CurrencyID,
                User = UserInfoProvider.GetUserInfo(userID),
                ShoppingCartShippingAddress = customerAddress,
                ShoppingCartShippingOptionID = shippingOption?.ShippingOptionID ?? 0
            };
            cart.SetValue("ShoppingCartCampaignID", campaignId);
            cart.SetValue("ShoppingCartProgramID", programId);
            cart.SetValue("ShoppingCartDistributorID", distributorId);
            cart.SetValue("ShoppingCartInventoryType", (int)cartType);
            ShoppingCartInfoProvider.SetShoppingCartInfo(cart);
            return cart?.ShoppingCartID ?? 0;
        }

        public void UpdateDistributorCart(DistributorCartItem distributorCartItem, CampaignsProduct product, CampaignProductType cartType = CampaignProductType.GeneralInventory)
        {
            ShoppingCartInfo cart = ShoppingCartInfoProvider.GetShoppingCartInfo(distributorCartItem.ShoppingCartID);
            ShoppingCartItemInfo item = cart.CartItems.Where(g => g.SKUID == product.SKUID).FirstOrDefault();
            if (cart != null)
            {
                if (item != null)
                {
                    ShoppingCartItemInfoProvider.UpdateShoppingCartItemUnits(item, distributorCartItem.Quantity);
                }
                else
                {
                    AddDistributorCartItem(cart.ShoppingCartID, distributorCartItem, product, cartType);
                }
            }
        }

        public void AddDistributorCartItem(int cartID, DistributorCartItem distributorCartItem, CampaignsProduct product, CampaignProductType cartType = CampaignProductType.GeneralInventory)
        {
            ShoppingCartInfo cart = ShoppingCartInfoProvider.GetShoppingCartInfo(cartID);
            if (cart != null)
            {
                ShoppingCartItemParameters parameters = new ShoppingCartItemParameters(product.SKUID, distributorCartItem.Quantity);
                parameters.CustomParameters.Add("CartItemCustomerID", distributorCartItem.DistributorID);
                ShoppingCartItemInfo cartItem = cart.SetShoppingCartItem(parameters);
                cartItem.SetValue("CartItemPrice", (cartType == CampaignProductType.GeneralInventory ? product.ActualPrice : product.EstimatedPrice));
                cartItem.SetValue("CartItemDistributorID", distributorCartItem.DistributorID);
                cartItem.SetValue("CartItemCampaignID", product.CampaignID);
                cartItem.SetValue("CartItemProgramID", product.ProgramID);
                cartItem.SetValue("ProductPageID", product.DocumentId);
                ShoppingCartItemInfoProvider.SetShoppingCartItemInfo(cartItem);
            }
        }

        public void DeleteDistributorCartItem(int cartID, int SKUID)
        {
            ShoppingCartInfo cart = ShoppingCartInfoProvider.GetShoppingCartInfo(cartID);
            ShoppingCartItemInfo item = cart.CartItems.Where(g => g.SKUID == SKUID).FirstOrDefault();
            if (cart != null && item != null)
            {
                ShoppingCartInfoProvider.RemoveShoppingCartItem(cart, item.CartItemID);
                ShoppingCartItemInfoProvider.DeleteShoppingCartItemInfo(item);
                if (cart.CartItems.Count == 0)
                {
                    ShoppingCartInfoProvider.DeleteShoppingCartInfo(cart);
                }
            }
        }

        public int GetDistributorCartCount(int userID, int campaignID, CampaignProductType cartType = CampaignProductType.GeneralInventory)
        {
            var query = new DataQuery("Ecommerce.Shoppingcart.GetShoppingCartCount");
            QueryDataParameters queryParams = new QueryDataParameters();
            queryParams.Add("@ShoppingCartUserID", userID);
            queryParams.Add("@ShoppingCartInventoryType", (int)cartType);
            queryParams.Add("@ShoppingCartCampaignID", campaignID);
            var countData = ConnectionHelper.ExecuteScalar(query.QueryText, queryParams, QueryTypeEnum.SQLQuery, true);
            return ValidationHelper.GetInteger(countData, default(int));
        }

        public decimal GetCartWeight(int cartId)
        {
            var cart = ShoppingCartInfoProvider.GetShoppingCartInfo(cartId);
            return (decimal)cart.CartItems.Sum(x => (x.CartItemUnits * x.UnitWeight));
        }

        public ShoppingCart GetShoppingCart(int cartId, string orderType)
        {
            var cart = ShoppingCartInfoProvider.GetShoppingCartInfo(cartId);
            if (cart == null)
            {
                return null;
            }

            return new ShoppingCart
            {
                Items = cart.CartItems
                    .Select(i => new ShoppingCartItem
                    {
                        SkuId = i.SKUID,
                        Quantity = i.CartItemUnits,
                        UnitPrice = ValidationHelper.GetDecimal(i.UnitPrice, default(decimal)),
                        TotalPrice = ValidationHelper.GetDecimal(i.TotalPrice, default(decimal))
                    })
                    .ToList(),
                PricedItemsTax = 0,
                TotalTax = 0,
                TotalPrice = OrderType.generalInventory.Equals(orderType) ? decimal.Zero : ValidationHelper.GetDecimal(cart.TotalItemsPrice, default(decimal))
            };
        }

        public void DeleteShoppingCart(int cartId)
        {
            ShoppingCartInfoProvider.DeleteShoppingCartInfo(cartId);
        }
    }
}