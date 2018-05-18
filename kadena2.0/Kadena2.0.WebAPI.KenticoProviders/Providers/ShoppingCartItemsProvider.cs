using AutoMapper;
using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.Helpers;
using CMS.IO;
using CMS.Localization;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.AmazonFileSystemProvider;
using Kadena.Models.Checkout;
using Kadena.Models.Product;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Linq;
using Kadena.Helpers;
using Kadena.Models.Common;
using Kadena.Models.SiteSettings;
using Kadena.Models.SubmitOrder;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders
{
    public class ShoppingCartItemsProvider : IShoppingCartItemsProvider
    {
        private readonly IKenticoResourceService resources;
        private readonly IKenticoDocumentProvider documents;
        private readonly IMapper mapper;
        private readonly IDynamicPriceRangeProvider dynamicPrices;
        private readonly IKenticoProductsProvider productProvider;
        private readonly IKenticoSiteProvider site;
        private readonly IKenticoUnitOfMeasureProvider units;

        public ShoppingCartItemsProvider(IKenticoResourceService resources, IKenticoDocumentProvider documents, IMapper mapper, IDynamicPriceRangeProvider dynamicPrices, IKenticoProductsProvider productProvider, IKenticoSiteProvider site, IKenticoUnitOfMeasureProvider units)
        {
            this.resources = resources ?? throw new ArgumentNullException(nameof(resources));
            this.documents = documents ?? throw new ArgumentNullException(nameof(documents));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.dynamicPrices = dynamicPrices ?? throw new ArgumentNullException(nameof(dynamicPrices));
            this.productProvider = productProvider ?? throw new ArgumentNullException(nameof(productProvider));
            this.site = site ?? throw new ArgumentNullException(nameof(site));
            this.units = units ?? throw new ArgumentNullException(nameof(units));
        }

        public int GetShoppingCartItemsCount()
        {
            return ECommerceContext.CurrentShoppingCart.CartItems.Count;
        }

        public CheckoutCartItem[] GetCheckoutCartItems(bool showPrices = true)
        {
            var displayProductionAndShipping = resources.GetSiteSettingsKey<bool>(Settings.KDA_Checkout_ShowProductionAndShipping);

            return GetCurrentShoppingCartItems()
                .Select(cartItem => MapCheckoutCartItem(cartItem, showPrices, displayProductionAndShipping))
                .ToArray();
        }

        public OrderCartItem[] GetOrderCartItems()
        {
            return GetCurrentShoppingCartItems()
                     .Select(cartItem => MapOrderCartItem(cartItem))
                     .ToArray();
        }

        private IEnumerable<ShoppingCartItemInfo> GetCurrentShoppingCartItems()
        {
            return ECommerceContext.CurrentShoppingCart.CartItems
                .Where(cartItem => !cartItem.IsProductOption);
        }

        private void CheckItemHasSku(ShoppingCartItemInfo i)
        {
            if (i.SKU == null)
            {
                throw new ArgumentNullException(nameof(i.SKU), "CartItem has null SKU");
            }
        }

        private CheckoutCartItem MapCheckoutCartItem(ShoppingCartItemInfo i, bool showPrices, bool displayProductionAndShipping)
        {
            CheckItemHasSku(i);

            var checkoutCartItem = new CheckoutCartItem()
            {
                Id = i.CartItemID,
                CartItemText = i.CartItemText,
                MailingListGuid = i.GetValue("MailingListGuid", Guid.Empty), // seem to be redundant parameter, microservice doesn't use it
                SKUName = !string.IsNullOrEmpty(i.CartItemText) ? i.CartItemText : i.SKU.SKUName,
                TotalTax = 0.0m,
                UnitPrice = showPrices ? (decimal)i.UnitPrice : 0.0m,
                UnitOfMeasureName = units.GetDisplayname(i.GetStringValue("UnitOfMeasure", UnitOfMeasure.DefaultUnit)),
                Image = productProvider.GetProductImagePath(i.GetIntegerValue("ProductPageID", 0)),
                ProductType = i.GetValue("ProductType", string.Empty),
                Quantity = i.CartItemUnits,
                TotalPrice = showPrices ? (decimal)i.UnitPrice * i.CartItemUnits : 0.0m,
                PriceText = showPrices ? string.Format("{0:#,0.00}", i.UnitPrice * i.CartItemUnits) : string.Empty,
                PricePrefix = showPrices ? resources.GetResourceString("Kadena.Checkout.ItemPricePrefix") : string.Empty,
                QuantityPrefix = resources.GetResourceString("Kadena.Checkout.QuantityPrefix"),
                MailingListName = i.GetValue("MailingListName", string.Empty),
                Template = !string.IsNullOrEmpty(i.CartItemText) ? i.CartItemText : i.SKU.SKUName,
                ProductPageId = i.GetIntegerValue("ProductPageID", 0),
                StockQuantity = i.SKU.SKUAvailableItems,
                MailingListPrefix = resources.GetResourceString("Kadena.Checkout.MailingListLabel"),
                TemplatePrefix = resources.GetResourceString("Kadena.Checkout.TemplateLabel"),
                ProductionTime = displayProductionAndShipping ? i.GetValue("ProductProductionTime", string.Empty) : null,
                ShipTime = displayProductionAndShipping ? i.GetValue("ProductShipTime", string.Empty) : null,
                Preview = new Button { Exists = false, Text = resources.GetResourceString("Kadena.Checkout.PreviewButton") },
                RequiresApproval = i.SKU.GetBooleanValue("SKUApprovalRequired", false),
                HiResPdfAllowed = i.SKU.GetBooleanValue("SKUHiResPdfDownloadEnabled", false),
                Options = GetItemOptions(i)
            };

            if (checkoutCartItem.IsTemplated)
            {
                var templateLowResSettingId = productProvider.GetProductByDocumentId(checkoutCartItem.ProductPageId)?.TemplateLowResSettingId ?? Guid.Empty;
                var previewUrl = UrlHelper.GetUrlForTemplatePreview(i.GetValue("ChilliEditorTemplateID", Guid.Empty), templateLowResSettingId);
                var previewAbsoluteUrl = site.GetAbsoluteUrl(previewUrl);

                checkoutCartItem.Preview.Url = previewAbsoluteUrl;
                checkoutCartItem.Preview.Exists = true;

                var editorUrl = documents.GetDocumentUrl(URLHelper.ResolveUrl(resources.GetSiteSettingsKey("KDA_Templating_ProductEditorUrl")));
                editorUrl = URLHelper.AddParameterToUrl(editorUrl, "documentId", checkoutCartItem.ProductPageId.ToString());
                editorUrl = URLHelper.AddParameterToUrl(editorUrl, "templateId", i.GetValue("ChilliEditorTemplateID", Guid.Empty).ToString());
                editorUrl = URLHelper.AddParameterToUrl(editorUrl, "workspaceid", i.GetValue("ProductChiliWorkspaceId", Guid.Empty).ToString());
                editorUrl = URLHelper.AddParameterToUrl(editorUrl, "containerId", checkoutCartItem.MailingListGuid.ToString());
                editorUrl = URLHelper.AddParameterToUrl(editorUrl, "quantity", checkoutCartItem.Quantity.ToString());
                editorUrl = URLHelper.AddParameterToUrl(editorUrl, "customName", URLHelper.URLEncode(checkoutCartItem.CartItemText));
                checkoutCartItem.EditorURL = editorUrl;

                checkoutCartItem.EmailProof = new Button()
                {
                    Exists = true,
                    Text = resources.GetResourceString("Kadena.EmailProof.ButtonLabel"),
                    Url = previewAbsoluteUrl
                };
            }

            return checkoutCartItem;
        }

        private OrderCartItem MapOrderCartItem(ShoppingCartItemInfo i)
        {
            CheckItemHasSku(i);

            var unitOfMeasure = i.GetStringValue("UnitOfMeasure", UnitOfMeasure.DefaultUnit);

            var orderCartItem = new OrderCartItem()
            {
                SKU = new OrderItemSku
                {
                    KenticoSKUID = i.SKUID,
                    Name = !string.IsNullOrEmpty(i.CartItemText) ? i.CartItemText : i.SKU.SKUName,
                    SKUNumber = i.SKU.SKUNumber,
                    HiResPdfAllowed = i.SKU.GetBooleanValue("SKUHiResPdfDownloadEnabled", false)
                },
                
                Artwork = i.GetValue("ArtworkLocation", string.Empty),
                MailingListGuid = i.GetValue("MailingListGuid", Guid.Empty),
                UnitPrice = (decimal)i.UnitPrice,
                UnitOfMeasureErpCode = units.GetUnitOfMeasure(unitOfMeasure).ErpCode,
                ProductType = i.GetValue("ProductType", string.Empty),
                Quantity = i.CartItemUnits,
                TotalPrice = (decimal)i.UnitPrice * i.CartItemUnits,
                SendPriceToErp = i.GetBooleanValue("SendPriceToErp", true),
                RequiresApproval = i.SKU.GetBooleanValue("SKUApprovalRequired", false),
                Options = GetItemOptions(i)
            };

            if (ProductTypes.IsOfType(orderCartItem.ProductType, ProductTypes.TemplatedProduct))
            {
                orderCartItem.ChiliProcess = new ChiliProcess
                {
                    TemplateId = i.GetValue("ChilliEditorTemplateID", Guid.Empty),
                    PdfSettings = i.GetValue("ProductChiliPdfGeneratorSettingsId", Guid.Empty),
                };
            }

            return orderCartItem;
        }

        private IEnumerable<ItemOption> GetItemOptions(ShoppingCartItemInfo i)
        {
            if (i.VariantParent != null)
            {
                var variant = new ProductVariant(i.SKUID);
                var attributes = variant.ProductAttributes.AsEnumerable();
                return attributes.Select(a => new ItemOption { Name = a.SKUOptionCategory.CategoryName, Value = a.SKUName });
            }
            else
            {
                return Enumerable.Empty<ItemOption>();
            }
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

            if (productType.Contains(ProductTypes.InventoryProduct) && item.SKU.SKUSellOnlyAvailable && quantity > item.SKU.SKUAvailableItems)
            {
                throw new Exception(string.Format(
                    ResHelper.GetString("Kadena.Product.SetQuantityForItemError", LocalizationContext.CurrentCulture.CultureCode), quantity, item.CartItemID));
            }

            var min = item.SKU?.SKUMinItemsInOrder ?? 0;
            var max = item.SKU?.SKUMaxItemsInOrder ?? 0;

            if ((min > 0 && quantity < min) || (max > 0 && quantity > max))
            {
                throw new Exception(string.Format(
                    ResHelper.GetString("Kadena.Product.SetQuantityForItemError", LocalizationContext.CurrentCulture.CultureCode), quantity, item.CartItemID));
            }


            var documentId = item.GetIntegerValue("ProductPageID", 0);
            var price = dynamicPrices.GetDynamicPrice(quantity, documentId);
            if (price > decimal.MinusOne)
            {
                item.CartItemPrice = (double)price;
            }

            ShoppingCartItemInfoProvider.UpdateShoppingCartItemUnits(item, quantity);
            cart.InvalidateCalculations();
            ShoppingCartInfoProvider.EvaluateShoppingCart(cart);
        }

        public void SaveCartItem(CartItemEntity item)
        {
            var cartItemInfo = ECommerceContext.CurrentShoppingCart.GetShoppingCartItem(item.CartItemGUID);

            cartItemInfo.CartItemText = item.CartItemText;
            cartItemInfo.CartItemUnits = item.SKUUnits;
            cartItemInfo.CartItemPrice = item.CartItemPrice.HasValue ? (double)item.CartItemPrice.Value : double.NaN;
            cartItemInfo.SetValue("ProductType", item.ProductType);
            cartItemInfo.SetValue("ProductPageID", item.ProductPageID);
            cartItemInfo.SetValue("ProductProductionTime", item.ProductProductionTime);
            cartItemInfo.SetValue("ProductShipTime", item.ProductShipTime);
            cartItemInfo.SetValue("ChilliEditorTemplateID", item.ChilliEditorTemplateID);
            cartItemInfo.SetValue("ChiliTemplateID", item.ChiliTemplateID);
            cartItemInfo.SetValue("ProductChiliPdfGeneratorSettingsId", item.ProductChiliPdfGeneratorSettingsId);
            cartItemInfo.SetValue("ProductChiliWorkspaceId", item.ProductChiliWorkspaceId);
            cartItemInfo.SetValue("ArtworkLocation", item.ArtworkLocation);
            cartItemInfo.SetValue("SendPriceToErp", item.SendPriceToErp);
            cartItemInfo.SetValue("UnitOfMeasure", item.UnitOfMeasure);

            ShoppingCartItemInfoProvider.SetShoppingCartItemInfo(cartItemInfo);

            foreach (ShoppingCartItemInfo option in cartItemInfo.ProductOptions)
            {
                ShoppingCartItemInfoProvider.SetShoppingCartItemInfo(option);
            }
        }

        public void SetArtwork(CartItemEntity cartItem, int documentId)
        {
            var productDocument = DocumentHelper.GetDocument(documentId, new TreeProvider(MembershipContext.AuthenticatedUser)) as SKUTreeNode;

            var guid = productDocument.GetStringValue("ProductArtwork", string.Empty);

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
                cartItem.ArtworkLocation = attachmentPath;
            }
        }

        public CartItemEntity GetOrCreateCartItem(NewCartItem newItem)
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

            // To return cart item with original quantity :
            var quantity = cartItemInfo.GetIntegerValue("SKUUnits", newItem.Quantity);
            cartItemInfo.SetValue("SKUUnits", quantity - newItem.Quantity );

            cartItemInfo.CartItemText = cartItemInfo.SKU.SKUName;
            cartItemInfo.CartItemPrice = cartItemInfo.SKU.SKUPrice;
            cartItemInfo.SetValue("ProductType", productDocument.GetStringValue("ProductType", string.Empty));
            cartItemInfo.SetValue("ProductPageID", productDocument.DocumentID);
            cartItemInfo.SetValue("ProductProductionTime", productDocument.GetStringValue("ProductProductionTime", string.Empty));
            cartItemInfo.SetValue("ProductShipTime", productDocument.GetStringValue("ProductShipTime", string.Empty));
            cartItemInfo.SetValue("ChilliEditorTemplateID", newItem.TemplateId);
            cartItemInfo.SetValue("ChiliTemplateID", productDocument.GetGuidValue("ProductChiliTemplateID", Guid.Empty));
            cartItemInfo.SetValue("ProductChiliPdfGeneratorSettingsId", productDocument.GetGuidValue("ProductChiliPdfGeneratorSettingsId", Guid.Empty));
            cartItemInfo.SetValue("ProductChiliWorkspaceId", productDocument.GetGuidValue("ProductChiliWorkgroupID", Guid.Empty));
            cartItemInfo.SetValue("SendPriceToErp", !cartItemInfo.SKU.GetBooleanValue("SKUDontSendPriceToERP", false));
            cartItemInfo.SetValue("UnitOfMeasure", productDocument.GetStringValue("SKUUnitOfMeasure", UnitOfMeasure.DefaultUnit));

            return mapper.Map<CartItemEntity>(cartItemInfo);
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