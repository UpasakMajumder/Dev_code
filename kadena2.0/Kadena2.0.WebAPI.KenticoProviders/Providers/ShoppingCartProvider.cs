using AutoMapper;
using CMS.DataEngine;
using CMS.CustomTables;
using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.Globalization;
using CMS.Helpers;
using CMS.IO;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.AmazonFileSystemProvider;
using Kadena.Models;
using Kadena.Models.Checkout;
using Kadena.Models.CustomerData;
using Kadena.Models.Product;
using Kadena.WebAPI.KenticoProviders.Contracts;
using Kadena2.WebAPI.KenticoProviders.Contracts.KadenaSettings;
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
        private readonly IMapper mapper;
        private readonly IShippingEstimationSettings estimationSettings;
        private readonly IDynamicPriceRangeProvider dynamicPrices;
        private readonly IKenticoProductsProvider productProvider;
        private readonly string campaignClassName = "KDA.CampaignsProduct";
        private readonly string CustomTableName = "KDA.UserAllocatedProducts";

        public ShoppingCartProvider(IKenticoResourceService resources, IKenticoLogger logger, IKenticoDocumentProvider documents, IMapper mapper, IShippingEstimationSettings estimationSettings, IDynamicPriceRangeProvider dynamicPrices, IKenticoProductsProvider productProvider)
        {
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }
            if (documents == null)
            {
                throw new ArgumentNullException(nameof(documents));
            }
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            if (estimationSettings == null)
            {
                throw new ArgumentNullException(nameof(estimationSettings));
            }
            if (dynamicPrices == null)
            {
                throw new ArgumentNullException(nameof(dynamicPrices));
            }
            if (productProvider == null)
            {
                throw new ArgumentNullException(nameof(productProvider));
            }

            this.resources = resources;
            this.logger = logger;
            this.documents = documents;
            this.mapper = mapper;
            this.estimationSettings = estimationSettings;
            this.dynamicPrices = dynamicPrices;
            this.productProvider = productProvider;
        }

        public DeliveryAddress GetCurrentCartShippingAddress()
        {
            var address = ECommerceContext.CurrentShoppingCart.ShoppingCartShippingAddress;
            return mapper.Map<DeliveryAddress>(address);
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
            int stateId = StateInfoProvider.GetStateInfoByCode(stateName).StateID;

            return new BillingAddress()
            {
                Street = streets,
                City = estimationSettings.SenderCity,
                Country = countryName,
                CountryId = countryId,
                Zip = estimationSettings.SenderPostal,
                State = stateName,
                StateId = stateId
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

                logger.LogInfo("GetShippingPrice", "Info", "Current shopping cart has no shipping address, therefore cannot estimate shipping prices");
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
            var cart = ECommerceContext.CurrentShoppingCart;
            var info = mapper.Map<AddressInfo>(address);
            info.AddressName = "TemporaryAddress";
            info.AddressPersonalName = "TemporaryAddress";
            info.AddressID = 0;
            info.AddressCustomerID = ECommerceContext.CurrentCustomer.CustomerID;

            info.Insert();
            cart.ShoppingCartShippingAddress = info;
            cart.SubmitChanges(true);
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
                    DesignFileKey = i.GetValue("ArtworkLocation", string.Empty),
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
                    Image = URLHelper.GetAbsoluteUrl(i.SKU.SKUImagePath),
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
                if (i.VariantParent != null)
                {
                    var variant = new ProductVariant(i.SKUID);
                    var attributes = variant.ProductAttributes.AsEnumerable();
                    cartItem.Options = attributes.Select(a => new ItemOption { Name = a.SKUOptionCategory.CategoryName, Value = a.SKUName });
                }
                else
                {
                    cartItem.Options = Enumerable.Empty<ItemOption>();
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


            ShoppingCartItemInfoProvider.UpdateShoppingCartItemUnits(item, quantity);

            var documentId = item.GetIntegerValue("ProductPageID", 0);
            var price = dynamicPrices.GetDynamicPrice(quantity, documentId);
            if (price > decimal.MinusOne)
            {
                item.CartItemPrice = (double)price;
            }

            cart.InvalidateCalculations();
            ShoppingCartInfoProvider.EvaluateShoppingCart(cart);
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

        private ShoppingCartInfo GetShoppingCart(int shoppingCartId = 0)
        {
            return shoppingCartId > 0
                ? ShoppingCartInfoProvider.GetShoppingCartInfo(shoppingCartId)
                : ECommerceContext.CurrentShoppingCart;
        }

        public void RemoveCurrentItemsFromStock(int shoppingCartId = 0)
        {
            var shoppingCart = GetShoppingCart(shoppingCartId);

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

        public void ClearCart(int shoppingCartId = 0)
        {
            var shoppingCart = GetShoppingCart(shoppingCartId);
            ShoppingCartInfoProvider.DeleteShoppingCartInfo(shoppingCart);
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
                AddressPersonalName = $"{customer.CustomerFirstName} {customer.CustomerLastName}",
                AddressPhone = address.Phone
            };
            info.AddressName = $"{info.AddressPersonalName}, {info.AddressLine1}, {info.AddressCity}";
            info.SetValue("AddressType", AddressType.Shipping.Code);
            info.SetValue("CompanyName", address.CustomerName);
            info.SetValue("Email", address.Email);

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
            var productDocument = DocumentHelper.GetDocument(newItem.DocumentId, new TreeProvider(MembershipContext.AuthenticatedUser)) as SKUTreeNode;
            var productType = productDocument.GetValue("ProductType", string.Empty);
            var isTemplatedType = ProductTypes.IsOfType(productType, ProductTypes.TemplatedProduct);

            var cartItem = EnsureCartItem(productDocument, newItem.Options.Values.Distinct(), newItem.TemplateId, addedAmount);

            if (productType.Contains(ProductTypes.InventoryProduct))
            {
                EnsureInventoryAmount(productDocument.GetIntegerValue("SKUAvailableItems", 0), addedAmount, cartItem.CartItemUnits);
            }

            var isMailingType = ProductTypes.IsOfType(productType, ProductTypes.MailingProduct);
            if (isMailingType)
            {
                if (mailingList?.AddressCount != addedAmount)
                {
                    throw new ArgumentException(resources.GetResourceString("Kadena.Product.InsertedAmmountValueIsNotValid"));
                }
                SetMailingList(cartItem, mailingList);
                SetAmount(cartItem, addedAmount);
            }

            SetArtwork(cartItem, productDocument.GetStringValue("ProductArtwork", string.Empty));

            SetDynamicPrice(cartItem, productDocument.GetStringValue("ProductDynamicPricing", string.Empty));
            SetCustomName(cartItem, newItem.CustomProductName);

            ShoppingCartItemInfoProvider.SetShoppingCartItemInfo(cartItem);
            foreach (ShoppingCartItemInfo option in cartItem.ProductOptions)
            {
                ShoppingCartItemInfoProvider.SetShoppingCartItemInfo(option);
            }
            return GetShoppingCartItems().FirstOrDefault(i => i.Id == cartItem.CartItemID);
        }

        private static void SetArtwork(ShoppingCartItemInfo cartItem, string guid)
        {
            if (!string.IsNullOrWhiteSpace(guid))
            {
                var attachmentPath = AttachmentURLProvider.GetFilePhysicalURL(SiteContext.CurrentSiteName, guid);
                if (!Path.HasExtension(attachmentPath))
                {
                    var attachment = DocumentHelper.GetAttachment(new Guid(guid), SiteContext.CurrentSiteName);
                    attachmentPath = $"{attachmentPath}{attachment.AttachmentExtension}";
                }
                var storageProvider = StorageHelper.GetStorageProvider(attachmentPath);
                if (storageProvider.IsExternalStorage && storageProvider.FileProviderObject.GetType() == typeof(AmazonFileSystemProvider.File))
                {
                    attachmentPath = PathHelper.GetObjectKeyFromPath(attachmentPath);
                }
                cartItem.SetValue("ArtworkLocation", attachmentPath);
            }
        }

        private void EnsureInventoryAmount(int availableQuantity, int addedQuantity, int resultedQuantity)
        {
            if (addedQuantity > availableQuantity)
            {
                throw new ArgumentException(resources.GetResourceString("Kadena.Product.LowerNumberOfAvailableProducts"));
            }
            else
            {
                if (resultedQuantity > availableQuantity)
                {
                    throw new ArgumentException(string.Format(resources.GetResourceString("Kadena.Product.ItemsInCartExceeded"),
                        resultedQuantity - addedQuantity, availableQuantity - resultedQuantity + addedQuantity));
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

        private void SetDynamicPrice(ShoppingCartItemInfo cartItem, string pricesJson)
        {
            var price = dynamicPrices.GetDynamicPrice(cartItem.CartItemUnits, pricesJson);
            if (price > decimal.MinusOne)
            {
                cartItem.CartItemPrice = decimal.ToDouble(price);
            }
        }

        private static ShoppingCartItemInfo EnsureCartItem(SKUTreeNode document, IEnumerable<int> attributes, Guid templateId, int quantity)
        {
            if (document == null)
            {
                return null;
            }

            var variant = VariantHelper.GetProductVariant(document.NodeSKUID, new ProductAttributeSet(attributes));
            ShoppingCartItemParameters parameters;
            if (variant != null && variant.SKUEnabled)
            {
                parameters = new ShoppingCartItemParameters(variant.SKUID, quantity);
            }
            else
            {
                parameters = new ShoppingCartItemParameters(document.NodeSKUID, quantity);
            }

            if (Guid.Empty != templateId)
            {
                var optionSku = EnsureTemplateOptionSKU();
                var option = new ShoppingCartItemParameters(optionSku.SKUID, quantity)
                {
                    Text = templateId.ToString()
                };

                parameters.ProductOptions.Add(option);
            }

            var cart = ECommerceContext.CurrentShoppingCart;
            ShoppingCartInfoProvider.SetShoppingCartInfo(cart);
            var cartItem = cart.SetShoppingCartItem(parameters);

            cartItem.CartItemText = cartItem.SKU.SKUName;
            cartItem.SetValue("ChilliEditorTemplateID", templateId);
            cartItem.SetValue("ChiliTemplateID", document.GetGuidValue("ProductChiliTemplateID", Guid.Empty));
            cartItem.SetValue("ProductType", document.GetStringValue("ProductType", string.Empty));
            cartItem.SetValue("ProductPageID", document.NodeID);
            cartItem.SetValue("ProductChiliPdfGeneratorSettingsId", document.GetGuidValue("ProductChiliPdfGeneratorSettingsId", Guid.Empty));
            cartItem.SetValue("ProductChiliWorkspaceId", document.GetGuidValue("ProductChiliWorkgroupID", Guid.Empty));
            cartItem.SetValue("ProductProductionTime", document.GetStringValue("ProductProductionTime", string.Empty));
            cartItem.SetValue("ProductShipTime", document.GetStringValue("ProductShipTime", string.Empty));

            return cartItem;
        }

        private static SKUInfo EnsureTemplateOptionSKU()
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
        public string UpdateCartQuantity(Distributor distributorData)
        {
            if (distributorData.ItemQuantity < 1)
            {
                throw new Exception(ResHelper.GetString("KDA.Cart.Update.MinimumQuantityError", LocalizationContext.CurrentCulture.CultureCode));
            }
            var shoppingCartItem = ShoppingCartItemInfoProvider.GetShoppingCartItemInfo(distributorData.CartItemId);
            if (distributorData.InventoryType == 1)
            {
                var shoppingCartIDs = ShoppingCartInfoProvider.GetShoppingCarts().WhereEquals("ShoppingCartUserID", distributorData.UserID).WhereEquals("ShoppingCartInventoryType", 1).ToList().Select(x => x.ShoppingCartID).ToList();
                var shoppingcartItems = ShoppingCartItemInfoProvider.GetShoppingCartItems().WhereIn("ShoppingCartID", shoppingCartIDs).WhereEquals("SKUID", shoppingCartItem.SKUID).ToList();
                int totalItems = 0;
                shoppingcartItems.ForEach(cartItem =>
                {
                    if (cartItem != null && cartItem.CartItemID != distributorData.CartItemId)
                    {
                        totalItems += cartItem.CartItemUnits;
                    }
                });
                var sku = SKUInfoProvider.GetSKUInfo(shoppingCartItem.SKUID);
                var currentProduct = DocumentHelper.GetDocuments(campaignClassName).WhereEquals("NodeSKUID", sku.SKUID).Columns("CampaignsProductID").FirstOrDefault();
                var productHasAllocation = currentProduct != null ? productProvider.IsProductHasAllocation(currentProduct.GetValue<int>("CampaignsProductID", default(int))) : false;
                var allocatedQuantityItem = GetAllocatedProductQuantityForUser(currentProduct.GetValue<int>("CampaignsProductID", default(int)), distributorData.UserID);
                var allocatedQuantity = allocatedQuantityItem != null ? allocatedQuantityItem.GetValue<int>("Quantity", default(int)) : default(int);
                if (sku.SKUAvailableItems < totalItems + distributorData.ItemQuantity)
                {
                    throw new Exception(ResHelper.GetString("KDA.Cart.Update.InsufficientStockMessage", LocalizationContext.CurrentCulture.CultureCode));
                }
                else if (allocatedQuantity < totalItems + distributorData.ItemQuantity && productHasAllocation)
                {
                    throw new Exception(ResHelper.GetString("Kadena.AddToCart.AllocatedProductQuantityError", LocalizationContext.CurrentCulture.CultureCode));
                }
            }
            if (shoppingCartItem != null)
            {
                shoppingCartItem.CartItemUnits = distributorData.ItemQuantity;
                shoppingCartItem.Update();
                return ResHelper.GetString("KDA.Cart.Update.Success");
            }
            else
            {
                throw new Exception(ResHelper.GetString("KDA.Cart.Update.Failure", LocalizationContext.CurrentCulture.CultureCode));
            }
        }


        public List<int> GetUserIDsWithShoppingCart(int campaignID, int productType)
        {
            return ShoppingCartInfoProvider.GetShoppingCarts().WhereEquals("ShoppingCartCampaignID", campaignID)
                                                                     .WhereEquals("ShoppingCartInventoryType", productType).ToList().Select(x => x.ShoppingCartUserID).Distinct().ToList();
        }

        public bool IsCartContainsInvalidProduct(int shoppingCartId = 0)
        {
            bool isValidCart = true;
            ShoppingCartInfo shoppingCart = GetShoppingCart(shoppingCartId);
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
            return ShoppingCartInfoProvider.GetShoppingCarts(SiteContext.CurrentSiteID).WhereEquals("ShoppingCartUserID", userID)?.ToList().Select(x => x.ShoppingCartID).ToList();
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
        private CustomTableItem GetAllocatedProductQuantityForUser(int productID, int userID)
        {
            return CustomTableItemProvider.GetItems(CustomTableName).WhereEquals("ProductID", productID).WhereEquals("UserID", userID).FirstOrDefault();

        }
        public ShoppingCartInfo GetShoppingCartByID(int cartID)
        {
            return ShoppingCartInfoProvider.GetShoppingCartInfo(cartID);
        }
        public List<int> GetShoppingCartIDs(WhereCondition where)
        {
            return ShoppingCartInfoProvider.GetShoppingCarts().Where(where)
                                                                  .Select(x => x.ShoppingCartID).ToList();
        }
        public List<ShoppingCartItemInfo> GetShoppingCartItemsByCartIDs(List<int> shoppingCartIDs)
        {
            return ShoppingCartItemInfoProvider.GetShoppingCartItems().WhereIn("ShoppingCartID", shoppingCartIDs)
                                                                                    .ToList();
        }
        public void UpdateBusinessUnit(ShoppingCartInfo cart, long businessUnitID)
        {
            cart.SetValue("BusinessUnitIDForDistributor", businessUnitID);
            cart.Update();
        }

        public List<int> GetShoppingCartIDByInventoryType(int inventoryType, int userID, int campaignID = 0)
        {
            return ShoppingCartInfoProvider.GetShoppingCarts(SiteContext.CurrentSiteID)
                                    .OnSite(SiteContext.CurrentSiteID)
                                    .WhereEquals("ShoppingCartUserID", userID)
                                    .WhereEquals("ShoppingCartCampaignID", campaignID)
                                    ?.ToList().Select(x => x.ShoppingCartID).ToList();
        }

        public int GetPreBuyDemandCount(int SKUID)
        {
            return ShoppingCartItemInfoProvider.GetShoppingCartItems()
                                         .OnSite(SiteContext.CurrentSiteID)
                                         .Where(x => x.SKUID.Equals(SKUID))
                                         .Sum(x => x.CartItemUnits);
        }
    }
}