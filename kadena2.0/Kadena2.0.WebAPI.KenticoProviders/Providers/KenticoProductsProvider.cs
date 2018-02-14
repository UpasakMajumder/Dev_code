using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using Kadena.Models.Product;
using CMS.DocumentEngine;
using CMS.Localization;
using System.Linq;
using CMS.Helpers;
using CMS.Ecommerce;
using CMS.Membership;
using System;
using CMS.SiteProvider;
using AutoMapper;
using CMS.DataEngine;
using CMS.CustomTables;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoProductsProvider : IKenticoProductsProvider
    {
        private readonly IMapper mapper;
        private readonly string CustomTableName = "KDA.UserAllocatedProducts";
        public KenticoProductsProvider(IMapper mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            this.mapper = mapper;
        }

        public List<ProductCategoryLink> GetCategories(string path)
        {
            var categoryNodes = GetDocuments(path, "KDA.ProductCategory", PathTypeEnum.Children).ToList();
            var categories = mapper.Map<List<ProductCategoryLink>>(categoryNodes);
            categories.ForEach(c => SetBorderInfo(c.Border));
            return categories;
        }

        public ProductCategoryLink GetCategory(string path)
        {
            var categoryNode = GetDocuments(path, "KDA.ProductCategory", PathTypeEnum.Single).SingleOrDefault();
            var category = mapper.Map<ProductCategoryLink>(categoryNode);
            SetBorderInfo(category?.Border);
            return category;
        }

        private void SetBorderInfo(Border border)
        {
            if (border?.Exists ?? false)
            {
                border.Value = SettingsKeyInfoProvider.GetValue("KDA_ProductThumbnailBorderStyle");
            }
        }

        public List<ProductLink> GetProducts(string path)
        {
            var pages = GetDocuments(path, "KDA.Product", PathTypeEnum.Children);

            return pages.Select(p => new ProductLink
            {
                Id = p.DocumentID,
                Title = p.DocumentName,
                Url = p.DocumentUrlPath,
                ImageUrl = URLHelper.ResolveUrl(p.GetValue("SKUImagePath", string.Empty), false),
                IsFavourite = false,
                Border = new Border
                {
                    Exists = !p.GetBooleanValue("ProductThumbnailBorderDisabled", false),
                },
                ParentPath = (p.Parent == null ? null : p.Parent.NodeAliasPath)
            }
            ).ToList();
        }

        public void UpdateSku(Sku sku)
        {
            var skuInfo = SKUInfoProvider.GetSKUInfo(sku.SkuId);
            if (skuInfo == null)
            {
                return;
            }

            skuInfo.SKUWeight = sku.Weight;
            skuInfo.SKUNeedsShipping = sku.NeedsShipping;
            skuInfo.Update();
        }

        private DocumentQuery GetDocuments(string path, string className, PathTypeEnum pathType)
        {
            return DocumentHelper.GetDocuments(className)
                            .Path(path, pathType)
                            .WhereEquals("ClassName", className)
                            .Culture(LocalizationContext.CurrentCulture.CultureCode)
                            .CheckPermissions()
                            .NestingLevel(1)
                            .OnCurrentSite()
                            .Published();
        }

        public string GetSkuImageUrl(int skuid)
        {
            if (skuid <= 0)
            {
                return string.Empty;
            }

            var sku = SKUInfoProvider.GetSKUInfo(skuid);
            var skuurl = sku?.SKUImagePath ?? string.Empty;

            return URLHelper.GetAbsoluteUrl(skuurl);
        }

        public void SetSkuAvailableQty(string skunumber, int availableItems)
        {
            var sku = SKUInfoProvider.GetSKUs().WhereEquals("SKUNumber", skunumber).FirstOrDefault();

            if (sku != null)
            {
                sku.SKUAvailableItems = availableItems;
                sku.SubmitChanges(false);
                sku.MakeComplete(true);
                sku.Update();
            }
        }

        public Product GetProductByNodeId(int nodeId)
        {
            var doc = DocumentHelper.GetDocument(nodeId, LocalizationContext.CurrentCulture.CultureCode,
                new TreeProvider(MembershipContext.AuthenticatedUser));
            return GetProduct(doc);
        }

        public Product GetProductByDocumentId(int documentId)
        {
            var doc = DocumentHelper.GetDocument(documentId, new TreeProvider(MembershipContext.AuthenticatedUser));
            return GetProduct(doc);
        }

        private static Product GetProduct(TreeNode doc)
        {
            var sku = SKUInfoProvider.GetSKUInfo(doc.NodeSKUID);

            if (doc == null)
            {
                return null;
            }

            var product = new Product()
            {
                Id = doc.DocumentID,
                Name = doc.DocumentName,
                DocumentUrl = doc.AbsoluteURL,
                Category = doc.Parent?.DocumentName ?? string.Empty,
                ProductType = doc.GetValue("ProductType", string.Empty),
                ProductChiliTemplateID = doc.GetValue<Guid>("ProductChiliTemplateID", Guid.Empty),
                ProductChiliWorkgroupID = doc.GetValue<Guid>("ProductChiliWorkgroupID", Guid.Empty)
            };

            if (sku != null)
            {
                product.SkuImageUrl = URLHelper.GetAbsoluteUrl(sku.SKUImagePath);
                product.StockItems = sku.SKUAvailableItems;
                product.Availability = sku.SKUAvailableItems > 0 ? "available" : "out";
                product.Weight = sku.SKUWeight;
            }

            return product;
        }

        public string GetProductStatus(int skuid)
        {
            if (!SettingsKeyInfoProvider.GetBoolValue("KDA_OrderDetailsShowProductStatus", SiteContext.CurrentSiteID) || skuid <= 0)
                return string.Empty;

            SKUInfo sku = SKUInfoProvider.GetSKUInfo(skuid);
            return sku != null ? (sku.SKUEnabled ? ResHelper.GetString("KDA.Common.Status.Active") : ResHelper.GetString("KDA.Common.Status.Inactive")) : string.Empty;
        }
        public void SetSkuAvailableQty(int skuid, int qty)
        {
            SKUInfo sku = SKUInfoProvider.GetSKUInfo(skuid);
            if (sku != null)
            {
                sku.SKUAvailableItems = sku.SKUAvailableItems-qty;
                sku.Update();
            }
        }
        public CustomTableItem GetAllocatedProductQuantityForUser(int productID, int userID)
        {
          return  CustomTableItemProvider.GetItems(CustomTableName).WhereEquals("ProductID", productID).WhereEquals("UserID", userID).FirstOrDefault();
        }
    }
}
