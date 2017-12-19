using AutoMapper;
using CMS.CustomTables;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.Globalization;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.Product;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders
{
    public class ShoppingCartProvider : IShoppingCartProvider
    {
        private readonly IKenticoResourceService resources;
        private readonly IKenticoLogger logger;
        private readonly IKenticoDocumentProvider documents;
        private readonly IMapper _mapper;

        public ShoppingCartProvider(IKenticoResourceService resources, IKenticoLogger logger, IKenticoDocumentProvider documents, IMapper mapper)
        {
            this.resources = resources;
            this.logger = logger;
            this.documents = documents;
            this._mapper = mapper;
        }

        public DeliveryAddress GetCurrentCartShippingAddress()
        {
            var address = ECommerceContext.CurrentShoppingCart.ShoppingCartShippingAddress;
            return _mapper.Map<DeliveryAddress>(address);
        }

        public BillingAddress GetDefaultBillingAddress()
        {
            var streets = new[]
            {
                SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderAddressLine1"),
                SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderAddressLine2")
            }.Where(i => !string.IsNullOrEmpty(i)).ToList();

            string countryName = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderCountry");
            string stateName = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderState");
            int countryId = CountryInfoProvider.GetCountryInfoByCode(countryName).CountryID;
            int stateId = StateInfoProvider.GetStateInfoByCode(stateName).StateID;

            return new BillingAddress()
            {
                Street = streets,
                City = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderCity"),
                Country = countryName,
                CountryId = countryId,
                Zip = SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".KDA_EstimateDeliveryPrice_SenderPostal"),
                State = stateName,
                StateId = stateId
            };
        }

        public DeliveryCarrier[] GetShippingCarriers()
        {
            var shippingOptions = GetShippingOptions();
            var carriers = CarrierInfoProvider.GetCarriers(SiteContext.CurrentSiteID).ToArray();

            var deliveryMethods = _mapper.Map<DeliveryCarrier[]>(carriers);

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
            var result = _mapper.Map<DeliveryOption[]>(services);
            foreach (var item in result)
            {
                item.Title = resources.ResolveMacroString(item.Title);
            }

            GetShippingPrice(result);
            return result;
        }

        public DeliveryOption GetShippingOption(int id)
        {
            var service = ShippingOptionInfoProvider.GetShippingOptionInfo(id);
            var result = _mapper.Map<DeliveryOption>(service);
            var carrier = CarrierInfoProvider.GetCarrierInfo(service.ShippingOptionCarrierID);
            result.CarrierCode = carrier.CarrierName;
            return result;
        }

        private void GetShippingPrice(DeliveryOption[] services)
        {
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
            var methods = _mapper.Map<PaymentMethod[]>(paymentOptionInfoCollection);

            foreach (var method in methods)
            {
                method.Title = resources.ResolveMacroString(method.DisplayName);
            }
            return methods;
        }

        public PaymentMethod GetPaymentMethod(int id)
        {
            var paymentInfo = PaymentOptionInfoProvider.GetPaymentOptionInfo(id);
            var method = _mapper.Map<PaymentMethod>(paymentInfo);
            method.Title = resources.ResolveMacroString(method.DisplayName);
            return method;
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

        public void SetShoppingCartAddress(DeliveryAddress address)
        {
            if (address != null)
            {
                if (address.Id > 0)
                {
                    SetShoppingCartAddress(address.Id);
                }
                else
                {
                    var cart = ECommerceContext.CurrentShoppingCart;

                    var info = _mapper.Map<AddressInfo>(address);
                    cart.ShoppingCartShippingAddress = info;
                    cart.SubmitChanges(true);
                }
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
        public int GetCurrentCartAddresId()
        {
            var address = ECommerceContext.CurrentShoppingCart.ShoppingCartShippingAddress;
            return address?.AddressID ?? 0;
        }

        public int GetCurrentCartShippingOptionId()
        {
            return ECommerceContext.CurrentShoppingCart.ShoppingCartShippingOptionID;
        }

        public int GetShoppingCartItemsCount()
        {
            return ECommerceContext.CurrentShoppingCart.CartItems.Count;
        }

        public CartItem[] GetShoppingCartItems(bool showPrices = true)
        {
            var items = ECommerceContext.CurrentShoppingCart.CartItems;
            var displayProductionAndShipping = resources.GetSettingsKey("KDA_Checkout_ShowProductionAndShipping").ToLower() == "true";

            var result = items
            .Where(cartItem => !cartItem.IsProductOption)
            .Select(i =>
            {
                var cartItem = new CartItem()
                {
                    Id = i.CartItemID,
                    CartItemText = i.CartItemText,
                    DesignFilePath = i.GetValue("ArtworkLocation", string.Empty),
                    MailingListGuid = i.GetValue("MailingListGuid", Guid.Empty), // seem to be redundant parameter, microservice doesn't use it
                    ChiliEditorTemplateId = i.GetValue("ChilliEditorTemplateID", Guid.Empty),
                    ProductChiliPdfGeneratorSettingsId = i.GetValue("ProductChiliPdfGeneratorSettingsId", Guid.Empty),
                    ProductChiliWorkspaceId = i.GetValue("ProductChiliWorkspaceId", Guid.Empty),
                    ChiliTemplateId = i.GetValue("ChiliTemplateID", Guid.Empty),
                    DesignFilePathTaskId = i.GetValue("DesignFilePathTaskId", Guid.Empty),
                    SKUName = !string.IsNullOrEmpty(i.CartItemText) ? i.CartItemText : i.SKU?.SKUName,
                    SKUNumber = i.SKU?.SKUNumber,
                    TotalTax = 0.0m,
                    UnitPrice = showPrices ? (decimal)i.UnitPrice : 0.0m,
                    UnitOfMeasure = "EA",
                    Image = i.GetGuidValue("ProductThumbnail", Guid.Empty) == Guid.Empty ? URLHelper.GetAbsoluteUrl(i.SKU.SKUImagePath) : URLHelper.GetAbsoluteUrl(string.Format("/CMSPages/GetFile.aspx?guid={0}", i.GetGuidValue("ProductThumbnail", Guid.Empty))),
                    ProductType = i.GetValue("ProductType", string.Empty),
                    Quantity = i.CartItemUnits,
                    TotalPrice = showPrices ? (decimal)i.UnitPrice * i.CartItemUnits : 0.0m,
                    PriceText = showPrices ? string.Format("{0:#,0.00}", i.UnitPrice * i.CartItemUnits) : string.Empty,
                    PricePrefix = showPrices ? resources.GetResourceString("Kadena.Checkout.ItemPricePrefix") : string.Empty,
                    QuantityPrefix = resources.GetResourceString("Kadena.Checkout.QuantityPrefix"),
                    MailingListName = i.GetValue("MailingListName", string.Empty),
                    Template = !string.IsNullOrEmpty(i.CartItemText) ? i.CartItemText : i.SKU.SKUName,
                    EditorTemplateId = i.GetValue("ChilliEditorTemplateID", string.Empty),
                    ProductPageId = i.GetIntegerValue("ProductPageID", 0),
                    SKUID = i.SKUID,
                    StockQuantity = i.SKU.SKUAvailableItems,
                    MailingListPrefix = resources.GetResourceString("Kadena.Checkout.MailingListLabel"),
                    TemplatePrefix = resources.GetResourceString("Kadena.Checkout.TemplateLabel"),
                    ProductionTime = displayProductionAndShipping ? i.GetValue("ProductProductionTime", string.Empty) : null,
                    ShipTime = displayProductionAndShipping ? i.GetValue("ProductShipTime", string.Empty) : null
                };
                if (cartItem.IsTemplated)
                {
                    cartItem.EditorURL = $@"{documents.GetDocumentUrl(resources.GetSettingsKey("KDA_Templating_ProductEditorUrl")?.TrimStart('~'))}
?nodeId={cartItem.ProductPageId}
&templateId={cartItem.EditorTemplateId}
&workspaceid={cartItem.ProductChiliWorkspaceId}
&containerId={cartItem.MailingListGuid}
&quantity={cartItem.Quantity}
&customName={URLHelper.URLEncode(cartItem.CartItemText)}";
                }
                return cartItem;
            }
            ).ToArray();

            return result;
        }

        public void RemoveCartItem(int id)
        {
            // Method approach inspired by \CMS\CMSModules\Ecommerce\Controls\Checkout\CartItemRemove.ascx.cs

            var cart = ECommerceContext.CurrentShoppingCart;
            var item = cart.CartItems.FirstOrDefault(i => i.CartItemID == id);

            if (item == null)
                return;

            // Delete all the children from the database if available
            foreach (ShoppingCartItemInfo scii in cart.CartItems)
            {
                if ((scii.CartItemBundleGUID == item.CartItemGUID) || (scii.CartItemParentGUID == item.CartItemGUID))
                {
                    ShoppingCartItemInfoProvider.DeleteShoppingCartItemInfo(scii);
                }
            }

            // Deletes the CartItem from the database
            ShoppingCartItemInfoProvider.DeleteShoppingCartItemInfo(item.CartItemGUID);
            // Delete the CartItem form the shopping cart object (session)
            ShoppingCartInfoProvider.RemoveShoppingCartItem(cart, item.CartItemGUID);

            // Recalculate shopping cart
            ShoppingCartInfoProvider.EvaluateShoppingCart(cart);
        }

        public void SetCartItemQuantity(int id, int quantity)
        {
            var item = ECommerceContext.CurrentShoppingCart.CartItems.Where(i => i.CartItemID == id).FirstOrDefault();

            if (item == null)
            {
                throw new ArgumentOutOfRangeException(string.Format(
                    ResHelper.GetString("Kadena.Product.ItemInCartNotFound", LocalizationContext.CurrentCulture.CultureCode),
                    id));
            }

            if (quantity < 1)
            {
                throw new ArgumentOutOfRangeException(string.Format(
                    ResHelper.GetString("Kadena.Product.NegativeQuantityError", LocalizationContext.CurrentCulture.CultureCode), quantity));
            }

            UpdateCartItemQuantity(item, quantity);
        }

        private void UpdateCartItemQuantity(ShoppingCartItemInfo item, int quantity)
        {
            var cart = ECommerceContext.CurrentShoppingCart;

            var productType = item.GetStringValue("ProductType", string.Empty);

            if (!productType.Contains(ProductTypes.InventoryProduct) && !productType.Contains(ProductTypes.POD) && !productType.Contains(ProductTypes.StaticProduct))
            {
                throw new Exception(ResHelper.GetString("Kadena.Product.QuantityForTypeError", LocalizationContext.CurrentCulture.CultureCode));
            }

            if (productType.Contains(ProductTypes.InventoryProduct) && quantity > item.SKU.SKUAvailableItems)
            {
                throw new ArgumentOutOfRangeException(string.Format(
                    ResHelper.GetString("Kadena.Product.SetQuantityForItemError", LocalizationContext.CurrentCulture.CultureCode), quantity, item.CartItemID));
            }

            var documentId = item.GetIntegerValue("ProductPageID", 0);
            var ranges = GetDynamicPricingRanges(documentId);

            if ((ranges?.Count() ?? 0) > 0)
            {
                var price = GetDynamicPrice(quantity, ranges);

                if (price != 0.0m)
                {
                    item.CartItemPrice = (double)price;
                    ShoppingCartItemInfoProvider.UpdateShoppingCartItemUnits(item, quantity);
                }
                else
                {
                    throw new Exception(ResHelper.GetString("Kadena.Product.QuantityOutOfRange", LocalizationContext.CurrentCulture.CultureCode));
                }
            }
            else
            {
                ShoppingCartItemInfoProvider.UpdateShoppingCartItemUnits(item, quantity);
            }

            cart.InvalidateCalculations();
            ShoppingCartInfoProvider.EvaluateShoppingCart(cart);
        }
        
        private decimal GetDynamicPrice(int quantity, IEnumerable<DynamicPricingRange> ranges)
        {
            if (ranges != null)
            {
                var matchingRange = ranges.FirstOrDefault(i => quantity >= i.MinVal && quantity <= i.MaxVal);
                if (matchingRange != null)
                {
                    return matchingRange.Price;
                }
            }
            return 0.0m;
        }

        private decimal GetDynamicPrice(TreeNode document, int quantity)
        {
            var ranges = GetDynamicPricingRanges(document);

            if (ranges != null && ranges.Count() > 0)
            {
                var matchingRange = ranges.FirstOrDefault(i => quantity >= i.MinVal && quantity <= i.MaxVal);
                if (matchingRange != null)
                {
                    return matchingRange.Price;
                }
                else
                {
                    return decimal.MinusOne;
                }
            }
            else
            {
                return (decimal)document.GetDoubleValue("SKUPrice", 0);
            }
        }

        private IEnumerable<DynamicPricingRange> GetDynamicPricingRanges(int documentId)
        {
            var document = DocumentHelper.GetDocument(documentId, new TreeProvider(MembershipContext.AuthenticatedUser));
            return GetDynamicPricingRanges(document);
        }

        private IEnumerable<DynamicPricingRange> GetDynamicPricingRanges(TreeNode document)
        {
            var rawJson = document?.GetStringValue("ProductDynamicPricing", string.Empty);
            var ranges = JsonConvert.DeserializeObject<List<DynamicPricingRange>>(rawJson ?? string.Empty);

            return ranges;
        }

        public void RemoveCurrentItemsFromStock()
        {
            var items = ECommerceContext.CurrentShoppingCart.CartItems;

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

        public void ClearCart()
        {
            ShoppingCartInfoProvider.DeleteShoppingCartInfo(ECommerceContext.CurrentShoppingCart);
        }

        public void SaveShippingAddress(DeliveryAddress address)
        {
            var customer = ECommerceContext.CurrentCustomer;
            var info = new AddressInfo
            {
                AddressID = address.Id,
                AddressLine1 = address.Address1,
                AddressLine2 = address.Address2,
                AddressCity = address.City,
                AddressStateID = address.State.Id,
                AddressCountryID = address.Country.Id,
                AddressZip = address.Zip,
                AddressCustomerID = customer.CustomerID,
                AddressPersonalName = $"{customer.CustomerFirstName} {customer.CustomerLastName}"
            };
            info.AddressName = $"{info.AddressPersonalName}, {info.AddressLine1}, {info.AddressCity}";
            info.SetValue("AddressType", AddressType.Shipping.Code);

            AddressInfoProvider.SetAddressInfo(info);
            address.Id = info.AddressID;
        }

        public double GetCurrentCartTotalItemsPrice()
        {
            return ECommerceContext.CurrentShoppingCart.TotalItemsPrice;
        }

        public double GetCurrentCartShippingCost()
        {
            return ECommerceContext.CurrentShoppingCart.Shipping;
        }
        
        public CartItem AddCartItem(NewCartItem newItem, MailingList mailingList = null)
        {
            var addedAmount = newItem?.Quantity ?? 0;
            if (addedAmount < 1)
            {
                throw new ArgumentException(resources.GetResourceString("Kadena.Product.InsertedAmmountValueIsNotValid"));
            }

            var productDocument = DocumentHelper.GetDocument(newItem.DocumentId, new TreeProvider(MembershipContext.AuthenticatedUser));
            var productType = productDocument.GetValue("ProductType", string.Empty);
            var isTemplatedType = ProductTypes.IsOfType(productType, ProductTypes.TemplatedProduct);

            var cartItem = ECommerceContext.CurrentShoppingCart.CartItems
                .FirstOrDefault(i => i.SKUID == productDocument.NodeSKUID && i.GetValue("ChilliEditorTemplateID", Guid.Empty) == newItem.TemplateId);
            var isNew = false;
            if (cartItem == null)
            {
                isNew = true;
                cartItem = isTemplatedType 
                    ? CreateCartItem(productDocument, newItem.TemplateId) 
                    : CreateCartItem(productDocument);
            }

            var existingAmountInCart = isNew ? 0 : cartItem.CartItemUnits;
            if (productType.Contains(ProductTypes.InventoryProduct))
            {
                EnsureInventoryAmount(productDocument, addedAmount, existingAmountInCart);
            }

            var isMailingType = ProductTypes.IsOfType(productType, ProductTypes.MailingProduct);
            if (isTemplatedType || isMailingType)
            {
                if (isMailingType)
                {
                    if (mailingList?.AddressCount != addedAmount)
                    {
                        throw new ArgumentException(resources.GetResourceString("Kadena.Product.InsertedAmmountValueIsNotValid"));
                    }
                    SetMailingList(cartItem, mailingList);
                }
                SetAmount(cartItem, addedAmount);
            }
            else
            {
                SetAmount(cartItem, addedAmount + existingAmountInCart);
            }

            var isPodType = ProductTypes.IsOfType(productType, ProductTypes.POD);
            if (isPodType)
            {
                SetArtwork(cartItem, productDocument.GetStringValue("ProductDigitalPrinting", string.Empty));
            }
            else
            {
                SetArtwork(cartItem, productDocument.GetStringValue("ProductArtworkLocation", string.Empty));
            }

            RefreshPrice(cartItem, productDocument);
            SetCustomName(cartItem, newItem.CustomProductName);

            ShoppingCartItemInfoProvider.SetShoppingCartItemInfo(cartItem);

            return GetShoppingCartItems().FirstOrDefault(i => i.Id == cartItem.CartItemID);
        }

        private void SetArtwork(ShoppingCartItemInfo cartItem, string artworkUrl)
        {
            cartItem.SetValue("ArtworkLocation", artworkUrl);
        }

        private void EnsureInventoryAmount(TreeNode productDocument, int addedAmount, int existingAmount)
        {
            var availableAmount = productDocument.GetIntegerValue("SKUAvailableItems", 0);
            if (addedAmount > availableAmount)
            {
                throw new ArgumentException(resources.GetResourceString("Kadena.Product.LowerNumberOfAvailableProducts"));
            }
            else
            {
                if (addedAmount + existingAmount > availableAmount)
                {
                    throw new ArgumentException(string.Format(resources.GetResourceString("Kadena.Product.ItemsInCartExceeded"),
                        existingAmount, availableAmount - existingAmount));
                }
            }
        }

        private static void SetMailingList(ShoppingCartItemInfo cartItem, MailingList mailingList)
        {
            if (mailingList != null)
            {
                cartItem.SetValue("MailingListName", mailingList.Name);
                cartItem.SetValue("MailingListGuid", mailingList.Id);
            }
        }

        private static void SetCustomName(ShoppingCartItemInfo cartItem, string customName)
        {
            if (!string.IsNullOrEmpty(customName))
            {
                cartItem.CartItemText = customName;
            }
        }

        private void RefreshPrice(ShoppingCartItemInfo cartItem, TreeNode document)
        {
            var dynamicUnitPrice = GetDynamicPrice(document, cartItem.CartItemUnits);
            if (dynamicUnitPrice == decimal.MinusOne)
            {
                throw new ArgumentException(resources.GetResourceString("Kadena.Product.QuantityOutOfRange"));
            }

            if (dynamicUnitPrice > decimal.Zero)
            {
                cartItem.CartItemPrice = decimal.ToDouble(dynamicUnitPrice);
            }
        }

        private ShoppingCartItemInfo CreateCartItem(TreeNode document)
        {
            return CreateCartItem(document, Guid.Empty);
        }

        private ShoppingCartItemInfo CreateCartItem(TreeNode document, Guid templateId)
        {
            if (document == null)
            {
                return null;
            }

            var sku = SKUInfoProvider.GetSKUInfo(document.NodeSKUID);

            var cart = ECommerceContext.CurrentShoppingCart;
            ShoppingCartInfoProvider.SetShoppingCartInfo(cart);

            var parameters = new ShoppingCartItemParameters(sku.SKUID, 1);

            if (Guid.Empty != templateId)
            {
                var optionSku = GetOrCreateTemplateOptionSKU();
                var option = new ShoppingCartItemParameters(optionSku.SKUID, 1)
                {
                    Text = templateId.ToString()
                };

                parameters.ProductOptions.Add(option);
            }

            var cartItem = cart.SetShoppingCartItem(parameters);

            cartItem.CartItemText = sku.SKUName;
            cartItem.SetValue("ChilliEditorTemplateID", templateId);
            cartItem.SetValue("ChiliTemplateID", document.GetGuidValue("ProductChiliTemplateID", Guid.Empty));
            cartItem.SetValue("ProductType", document.GetStringValue("ProductType", string.Empty));
            cartItem.SetValue("ProductPageID", document.NodeID);
            cartItem.SetValue("ProductChiliPdfGeneratorSettingsId", document.GetGuidValue("ProductChiliPdfGeneratorSettingsId", Guid.Empty));
            cartItem.SetValue("ProductChiliWorkspaceId", document.GetGuidValue("ProductChiliWorkgroupID", Guid.Empty));
            cartItem.SetValue("ProductThumbnail", document.GetGuidValue("ProductThumbnail", Guid.Empty));
            cartItem.SetValue("ProductProductionTime", document.GetStringValue("ProductProductionTime", string.Empty));
            cartItem.SetValue("ProductShipTime", document.GetStringValue("ProductShipTime", string.Empty));

            return cartItem;
        }

        private SKUInfo GetOrCreateTemplateOptionSKU()
        {
            const string optionName = "TemplatedProductOption";
            var optionSku = SKUInfoProvider.GetSKUs()
                .WhereEquals(nameof(SKUInfo.SKUName), optionName)
                .FirstOrDefault();
            if (optionSku == null)
            {
                optionSku = new SKUInfo
                {
                    SKUName = optionName,
                    SKUPrice = 0,
                    SKUEnabled = true,
                    SKUProductType = SKUProductTypeEnum.Text
                };

                SKUInfoProvider.SetSKUInfo(optionSku);
            }

            return optionSku;
        }

        private static void SetAmount(ShoppingCartItemInfo cartItem, int amount)
        {
            cartItem.CartItemUnits = amount;
        }

        public string MapOrderStatus(string microserviceStatus)
        {
            var genericStatusItem = CustomTableItemProvider.GetItems("KDA.OrderStatusMapping")
                .FirstOrDefault(i => i["MicroserviceStatus"].ToString().ToLower() == microserviceStatus.ToLower());

            var resourceKey = genericStatusItem?.GetValue("GenericStatus")?.ToString();

            return string.IsNullOrEmpty(resourceKey) ? microserviceStatus : resources.GetResourceString(resourceKey);
        }        
    }
}