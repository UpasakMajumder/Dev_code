using AutoMapper;
using CMS.Ecommerce;
using Kadena.Models.Checkout;
using Kadena.Models.Product;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Linq;
using Kadena.Models.SubmitOrder;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders
{
    public class OrderCartItemsProvider : IOrderCartItemsProvider
    {
        private readonly IKenticoUnitOfMeasureProvider units;

        public OrderCartItemsProvider(IKenticoUnitOfMeasureProvider units)
        {
            this.units = units ?? throw new ArgumentNullException(nameof(units));
        }

        public OrderCartItem[] GetOrderCartItems()
        {
            return ECommerceContext.CurrentShoppingCart.CartItems
                     .Where(cartItem => !cartItem.IsProductOption)
                     .Select(cartItem => MapOrderCartItem(cartItem))
                     .ToArray();
        }

        private OrderCartItem MapOrderCartItem(ShoppingCartItemInfo i)
        {
            if (i.SKU == null)
            {
                throw new ArgumentNullException(nameof(i.SKU), "Order item has null SKU");
            }

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
                UnitPrice = (decimal)i.UnitPrice,
                UnitOfMeasureErpCode = units.GetUnitOfMeasure(unitOfMeasure).ErpCode,
                ProductType = i.GetValue("ProductType", string.Empty),
                Quantity = i.CartItemUnits,
                TotalPrice = (decimal)i.UnitPrice * i.CartItemUnits,
                SendPriceToErp = i.GetBooleanValue("SendPriceToErp", true),
                RequiresApproval = i.SKU.GetBooleanValue("SKUApprovalRequired", false),
                Options = GetItemOptions(i)
            };

            if (ProductTypes.IsOfType(orderCartItem.ProductType, ProductTypes.MailingProduct))
            {
                orderCartItem.MailingList = new MailingList
                {
                    MailingListID = i.GetValue("MailingListGuid", Guid.Empty)
                };
            }

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
    }
}