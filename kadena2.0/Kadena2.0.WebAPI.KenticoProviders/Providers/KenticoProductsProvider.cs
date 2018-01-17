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

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoProductsProvider : IKenticoProductsProvider
    {
        private readonly IMapper mapper;

        public KenticoProductsProvider(IMapper mapper)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public List<ProductCategoryLink> GetCategories(string path)
        {
            var categories = GetDocuments(path, "KDA.ProductCategory", PathTypeEnum.Children).ToList();
            return mapper.Map<List<ProductCategoryLink>>(categories);
        }

        public ProductCategoryLink GetCategory(string path)
        {
            var category = GetDocuments(path, "KDA.ProductCategory", PathTypeEnum.Single).SingleOrDefault();
            return mapper.Map<ProductCategoryLink>(category);
        }

        public List<ProductLink> GetProducts(string path)
        {
            var bordersEnabledOnSite = SettingsKeyInfoProvider.GetBoolValue("KDA_ProductThumbnailBorderEnabled");
            var borderEnabledOnParentCategory =  GetCategory(path)?.ProductBordersEnabled ?? false;
            var borderStyle = SettingsKeyInfoProvider.GetValue("KDA_ProductThumbnailBorderStyle");
            var pages = GetDocuments(path, "KDA.Product", PathTypeEnum.Children);

            return pages.Select(p => new ProductLink
            {
                Id = p.DocumentID,
                Title = p.DocumentName,
                Url = p.DocumentUrlPath,
                ImageUrl = URLHelper.GetAbsoluteUrl(p.GetValue("ProductThumbnail", string.Empty) == string.Empty ?
                                                    p.GetValue("SKUImagePath", string.Empty) :
                                                    "/CMSPages/GetFile.aspx?guid=" + p.GetValue("ProductThumbnail")),
                IsFavourite = false,
                Border = new Border
                {
                    Exists = p.GetBooleanValue("ProductThumbnailBorder", false) && bordersEnabledOnSite && borderEnabledOnParentCategory,
                    Value = borderStyle
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

        private DocumentQuery GetDocuments(string path, string className, PathTypeEnum pathType )
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
                return string.Empty;

            var sku = SKUInfoProvider.GetSKUInfo(skuid);
            var document = DocumentHelper.GetDocument(new NodeSelectionParameters { Where = "NodeSKUID = " + skuid, SiteName = SiteContext.CurrentSiteName, CultureCode = LocalizationContext.PreferredCultureCode, CombineWithDefaultCulture = false }, new TreeProvider(MembershipContext.AuthenticatedUser));
            var skuurl = sku?.SKUImagePath ?? string.Empty;

            if ((document?.GetGuidValue("ProductThumbnail", Guid.Empty) ?? Guid.Empty) != Guid.Empty)
            {
                return URLHelper.GetAbsoluteUrl(string.Format("/CMSPages/GetFile.aspx?guid={0}", document.GetGuidValue("ProductThumbnail", Guid.Empty)));
            }
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

        public string GetProductTeaserImageUrl(int documentId)
        {
            var result = string.Empty;

            var doc = DocumentHelper.GetDocument(documentId, LocalizationContext.CurrentCulture.CultureCode, new TreeProvider(MembershipContext.AuthenticatedUser));
            if ((doc?.GetGuidValue("ProductThumbnail", Guid.Empty) ?? Guid.Empty) != Guid.Empty)
            {
                result = URLHelper.GetAbsoluteUrl(string.Format("/CMSPages/GetFile.aspx?guid={0}", doc.GetGuidValue("ProductThumbnail", Guid.Empty)));
            }
            return result;
        }
    }
}
