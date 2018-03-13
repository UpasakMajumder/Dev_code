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
using Kadena.Helpers;
using Kadena.Models.Common;
using Kadena.Models.AddToCart;

namespace Kadena.WebAPI.KenticoProviders
{
    public class ShoppingCartItemsProvider : IShoppingCartItemsProvider
    {
        private readonly IKenticoResourceService resources;
        private readonly IKenticoDocumentProvider documents;
        private readonly IMapper mapper;
        private readonly IDynamicPriceRangeProvider dynamicPrices;
        private readonly IKenticoProductsProvider productProvider;

        public ShoppingCartItemsProvider(IKenticoResourceService resources, IKenticoDocumentProvider documents, IMapper mapper, IDynamicPriceRangeProvider dynamicPrices, IKenticoProductsProvider productProvider)
        {
            if (resources == null)
            {
                throw new ArgumentNullException(nameof(resources));
            }
            if (documents == null)
            {
                throw new ArgumentNullException(nameof(documents));
            }
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
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
            this.documents = documents;
            this.mapper = mapper;
            this.dynamicPrices = dynamicPrices;
            this.productProvider = productProvider;
        }

        public int GetShoppingCartItemsCount()
        {
            return ECommerceContext.CurrentShoppingCart.CartItems.Count;
        }

        public CartItem[] GetShoppingCartItems(bool showPrices = true)
        {
            var displayProductionAndShipping = resources.GetSettingsKey("KDA_Checkout_ShowProductionAndShipping").ToLower() == "true";

            return ECommerceContext.CurrentShoppingCart.CartItems
                .Where(cartItem => !cartItem.IsProductOption)
                .Select(cartItem => MapCartItem(cartItem, showPrices, displayProductionAndShipping))
                .ToArray();
        }

        private CartItem MapCartItem(ShoppingCartItemInfo i, bool showPrices, bool displayProductionAndShipping)
        {
            var cartItem = new CartItem()
            {
                Id = i.CartItemID,
                CartItemText = i.CartItemText,
                Artwork = i.GetValue("ArtworkLocation", string.Empty),
                MailingListGuid = i.GetValue("MailingListGuid", Guid.Empty), // seem to be redundant parameter, microservice doesn't use it
                ProductChiliWorkspaceId = i.GetValue("ProductChiliWorkspaceId", Guid.Empty),
                ChiliTemplateId = i.GetValue("ChiliTemplateID", Guid.Empty),
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
                ProductPageId = i.GetIntegerValue("ProductPageID", 0),
                SKUID = i.SKUID,
                StockQuantity = i.SKU.SKUAvailableItems,
                MailingListPrefix = resources.GetResourceString("Kadena.Checkout.MailingListLabel"),
                TemplatePrefix = resources.GetResourceString("Kadena.Checkout.TemplateLabel"),
                ProductionTime = displayProductionAndShipping ? i.GetValue("ProductProductionTime", string.Empty) : null,
                ShipTime = displayProductionAndShipping ? i.GetValue("ProductShipTime", string.Empty) : null,
                Preview = new Button { Exists = false, Text = resources.GetResourceString("Kadena.Checkout.PreviewButton") }
            };

            if (cartItem.IsTemplated)
            {
                cartItem.ChiliProcess = new ChiliProcess
                {
                    TemplateId = i.GetValue("ChilliEditorTemplateID", Guid.Empty),
                    PdfSettings = i.GetValue("ProductChiliPdfGeneratorSettingsId", Guid.Empty),
                };
                var product = productProvider.GetProductByNodeId(cartItem.ProductPageId);
                cartItem.Preview.Url = UrlHelper.GetUrlForTemplatePreview(cartItem.ChiliProcess.TemplateId, product.TemplateLowResSettingId);
                cartItem.Preview.Exists = true;

                var editorUrl = documents.GetDocumentUrl(URLHelper.ResolveUrl(resources.GetSettingsKey("KDA_Templating_ProductEditorUrl")));
                editorUrl = URLHelper.AddParameterToUrl(editorUrl, "nodeId", cartItem.ProductPageId.ToString());
                editorUrl = URLHelper.AddParameterToUrl(editorUrl, "templateId", cartItem.ChiliProcess.TemplateId.ToString());
                editorUrl = URLHelper.AddParameterToUrl(editorUrl, "workspaceid", cartItem.ProductChiliWorkspaceId.ToString());
                editorUrl = URLHelper.AddParameterToUrl(editorUrl, "containerId", cartItem.MailingListGuid.ToString());
                editorUrl = URLHelper.AddParameterToUrl(editorUrl, "quantity", cartItem.Quantity.ToString());
                editorUrl = URLHelper.AddParameterToUrl(editorUrl, "customName", URLHelper.URLEncode(cartItem.CartItemText));
                cartItem.EditorURL = editorUrl;
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

        public void SaveCartItem(CartItem item)
        {
            var cartItemInfo = ShoppingCartItemInfoProvider.GetShoppingCartItemInfo(item.Id);

            cartItemInfo.CartItemText = item.CustomName;
            cartItemInfo.CartItemUnits = item.Quantity;
            cartItemInfo.SetValue("MailingListName", item.MailingListName);
            cartItemInfo.SetValue("MailingListGuid", item.MailingListGuid);

            ShoppingCartItemInfoProvider.SetShoppingCartItemInfo(cartItemInfo);
            foreach (ShoppingCartItemInfo option in cartItemInfo.ProductOptions)
            {
                ShoppingCartItemInfoProvider.SetShoppingCartItemInfo(option);
            }
        }

        public void SetArtwork(CartItem cartItem, string guid)
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
                cartItem.Artwork = attachmentPath;
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

        public CartItem EnsureCartItem(NewCartItem newItem)
        {
            var productDocument = DocumentHelper.GetDocument(newItem.DocumentId, new TreeProvider(MembershipContext.AuthenticatedUser)) as SKUTreeNode;

            if (productDocument == null)
            {
                return null;
            }

            var attributes = newItem.Options.Values.Distinct();

            var variantSkuInfo = VariantHelper.GetProductVariant(productDocument.NodeSKUID, new ProductAttributeSet(attributes));

            ShoppingCartItemParameters parameters;

            
            if (variantSkuInfo != null && variantSkuInfo.SKUEnabled)
            {
                parameters = new ShoppingCartItemParameters(variantSkuInfo.SKUID, newItem.Quantity);
            }
            else
            {
                parameters = new ShoppingCartItemParameters(productDocument.NodeSKUID, newItem.Quantity);
            }

            if (Guid.Empty != newItem.TemplateId)
            {
                var optionSku = EnsureTemplateOptionSKU();
                var option = new ShoppingCartItemParameters(optionSku.SKUID, newItem.Quantity)
                {
                    Text = newItem.TemplateId.ToString()
                };

                parameters.ProductOptions.Add(option);
            }

            var cart = ECommerceContext.CurrentShoppingCart;
            ShoppingCartInfoProvider.SetShoppingCartInfo(cart);
            var cartItemInfo = cart.SetShoppingCartItem(parameters);

            var cartItem = MapCartItem(cartItemInfo, true, true);

            cartItem.CartItemText = cartItemInfo.SKU.SKUName;
            cartItem.DynamicPricing = productDocument.GetStringValue("ProductDynamicPricing", string.Empty);

           return cartItem;
        }

        private SKUInfo EnsureTemplateOptionSKU()
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
    }
}