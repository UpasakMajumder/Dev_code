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
using Kadena.Models.SiteSettings;
using Kadena.Models.ProductOptions;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoProductsProvider : IKenticoProductsProvider
    {
        private readonly IMapper mapper;
        private readonly IKenticoProgramsProvider programsProvider;
        private const string CustomTableName = "KDA.UserAllocatedProducts";

        public KenticoProductsProvider(IMapper mapper, IKenticoProgramsProvider programsProvider)
        {
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.programsProvider = programsProvider ?? throw new ArgumentNullException(nameof(programsProvider));
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
                border.Value = SettingsKeyInfoProvider.GetValue(Settings.KDA_ProductThumbnailBorderStyle, new SiteInfoIdentifier(SiteContext.CurrentSiteID));
            }
        }

        public ProductLink GetProductLinkBySkuid(int skuid)
        {
            var p = GetDocBySkuid(skuid);

            return new ProductLink
            {
                Id = p.DocumentID,
                Title = p.DocumentName,
                Url = p.DocumentUrlPath,
                Order = p.NodeOrder,
                ImageUrl = URLHelper.ResolveUrl(p.GetValue("ProductImage", string.Empty), false),
                IsFavourite = false,
                Border = new Border
                {
                    Exists = !p.GetBooleanValue("ProductThumbnailBorderDisabled", false),
                },
                ParentPath = p.Parent?.NodeAliasPath
            };
        }

        public List<ProductLink> GetProducts(string path)
        {
            var pages = GetDocuments(path, "KDA.Product", PathTypeEnum.Children);

            return pages.Select(p => new ProductLink
            {
                Id = p.DocumentID,
                Title = p.DocumentName,
                Url = p.DocumentUrlPath,
                Order = p.NodeOrder,
                ImageUrl = URLHelper.ResolveUrl(p.GetValue("ProductImage", string.Empty), false),
                IsFavourite = false,
                Border = new Border
                {
                    Exists = !p.GetBooleanValue("ProductThumbnailBorderDisabled", false),
                },
                ParentPath = (p.Parent == null ? null : p.Parent.NodeAliasPath)
            }
            ).ToList();
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

        [Obsolete("SKUImage is not used anymore, now ir is page's ProductImage")]
        public string GetSkuImageUrl(int skuid)
        {
            if (skuid <= 0)
            {
                return string.Empty;
            }

            var document = GetDocBySkuid(skuid);

            // check if SKU is product variant and get image from parent SKU
            if (document == null)
            {
                var sku = SKUInfoProvider.GetSKUInfo(skuid);
                if (sku.IsProductVariant)
                    document = GetDocBySkuid(sku.SKUParentSKUID);
            }

            var imgurl = document?.GetStringValue("ProductImage", string.Empty) ?? string.Empty;

            return URLHelper.GetAbsoluteUrl(imgurl);
        }

        public string GetProductImagePath(int productPageId)
        {
            var productPage = DocumentHelper.GetDocument(productPageId, new TreeProvider(MembershipContext.AuthenticatedUser));

            if (productPage == null)
            {
                return string.Empty;
            }

            var imagePath = productPage.GetValue("ProductImage", string.Empty);

            return URLHelper.GetAbsoluteUrl(imagePath);
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

        public Product[] GetProductsByDocumentIds(int[] documentIds)
        {
            var docs = DocumentHelper.GetDocuments("KDA.Product").WhereIn("DocumentID", documentIds).ToArray();
            var skuIds = docs.Select(d => d.NodeSKUID).ToArray();
            var skuInfos = SKUInfoProvider.GetSKUs().WhereIn("SKUId", skuIds).ToArray();
            var products = mapper.Map<Product[]>(docs);

            foreach (var product in products)
            {
                var sku = skuInfos.FirstOrDefault(s => s.SKUID == product.SkuId) ?? throw new Exception($"SKU with ID '{product.SkuId}' not found");
                SetDefaultSettingsId(product);
                SetSkuProperties(product, sku);
            }

            return products;
        }

        private TreeNode GetDocBySkuid(int skuid)
        {
            return DocumentHelper.GetDocument(new NodeSelectionParameters { Where = "NodeSKUID = " + skuid, SiteName = SiteContext.CurrentSiteName, CultureCode = LocalizationContext.PreferredCultureCode, CombineWithDefaultCulture = false }, new TreeProvider(MembershipContext.AuthenticatedUser));
        }

        private Product GetProduct(TreeNode doc)
        {
            if (doc == null)
            {
                return null;
            }

            var sku = SKUInfoProvider.GetSKUInfo(doc.NodeSKUID);

            var product = this.mapper.Map<Product>(doc);

            SetDefaultSettingsId(product);

            SetSkuProperties(product, sku);

            return product;
        }

        private void SetDefaultSettingsId(Product product)
        {
            if (product.IsTemplateLowResSettingMissing)
            {
                var defaultId = SettingsKeyInfoProvider.GetValue("KDA_DefaultLowResSettingsId", new SiteInfoIdentifier(SiteContext.CurrentSiteID));
                product.TemplateLowResSettingId = Guid.Parse(defaultId);
            }
        }

        private void SetSkuProperties(Product product, SKUInfo sku)
        {
            if (sku != null)
            {
                product.SkuImageUrl = URLHelper.GetAbsoluteUrl(sku.SKUImagePath);
                product.StockItems = sku.SKUAvailableItems;
                product.Availability = sku.SKUAvailableItems > 0 ? "available" : "out";
                product.Weight = sku.SKUWeight;
                product.HiResPdfDownloadEnabled = sku.GetBooleanValue("SKUHiResPdfDownloadEnabled", false);
                product.SkuNumber = sku.SKUNumber;
            }
        }

        public string GetProductStatus(int skuid)
        {
            if (!SettingsKeyInfoProvider.GetBoolValue("KDA_OrderDetailsShowProductStatus", SiteContext.CurrentSiteID) || skuid <= 0)
                return string.Empty;

            SKUInfo sku = SKUInfoProvider.GetSKUInfo(skuid);
            return sku != null ? (sku.SKUEnabled ? ResHelper.GetString("KDA.Common.Status.Active") : ResHelper.GetString("KDA.Common.Status.Inactive")) : string.Empty;
        }

        public int GetAllocatedProductQuantityForUser(int skuId, int userID)
        {
            var productID = GetCampaignProductIDBySKUID(skuId);
            var allocatedItems = GetAllocatedProductQuantityForUser(userID, new List<int> { productID });
            if (allocatedItems.ContainsKey(productID))
            {
                return allocatedItems[productID];
            }

            return -1;
        }

        public void UpdateAllocatedProductQuantityForUser(int skuId, int userID, int quantity)
        {
            var productId = GetCampaignProductIDBySKUID(skuId);

            if (productId > 0)
            {
                DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(CustomTableName);
                if (customTable != null)
                {
                    var customTableData = CustomTableItemProvider
                        .GetItems(CustomTableName)
                        .WhereEquals("ProductID", productId)
                        .WhereEquals("UserID", userID)
                        .FirstOrDefault();
                    if (customTableData != null)
                    {
                        customTableData.SetValue("Quantity", customTableData.GetIntegerValue("Quantity", 0) - quantity);
                        customTableData.Update();
                    }
                }

            }
        }

        public List<CampaignsProduct> GetCampaignsProductSKUIDs(List<int> programIds)
        {
            var productNodes = new TreeProvider(MembershipContext.AuthenticatedUser).SelectNodes("KDA.CampaignsProduct")
                                    .WhereIn("ProgramID", programIds)
                                    .OnCurrentSite();
            if (productNodes != null && productNodes.HasResults() && productNodes.TypedResult.Items.Count > 0)
            {
                return productNodes.TypedResult.Items.ToList().Select(x => mapper.Map<CampaignsProduct>(x)).ToList();
            }
            else
            {
                return new List<CampaignsProduct>();
            }
        }

        public OptionCategory GetOptionCategory(string codeName)
        {
            var category = BaseAbstractInfoProvider
                .GetInfoByName(OptionCategoryInfo.OBJECT_TYPE, codeName);
            return mapper.Map<OptionCategory>(category);
        }

        public Product GetProductBySkuId(int skuId)
        {
            var node = new TreeProvider(MembershipContext.AuthenticatedUser)
                .SelectSingleNode(
                    new NodeSelectionParameters { Where = $"{nameof(TreeNode.NodeSKUID)}={skuId}" }
                );
            if (node != null)
            {
                return GetProductByNodeId(node.NodeID);
            }

            return null;
        }

        private static int GetCampaignProductIDBySKUID(int skuid)
        {
            var document = DocumentHelper.GetDocument(new NodeSelectionParameters
            {
                Where = $"NodeSKUID = {skuid}",
                SiteName = SiteContext.CurrentSiteName,
                CultureCode = LocalizationContext.PreferredCultureCode,
                CombineWithDefaultCulture = false
            }, new TreeProvider(MembershipContext.AuthenticatedUser));
            return document?.GetIntegerValue("CampaignsProductID", default(int)) ?? default(int);
        }

        public CampaignsProduct GetCampaignProduct(int skuid)
        {
            var document = DocumentHelper.GetDocument(new NodeSelectionParameters { Where = "NodeSKUID = " + skuid, SiteName = SiteContext.CurrentSiteName, CultureCode = LocalizationContext.PreferredCultureCode, CombineWithDefaultCulture = false }, new TreeProvider(MembershipContext.AuthenticatedUser));
            SKUInfo sku = SKUInfoProvider.GetSKUInfo(skuid);
            var campaignId = programsProvider.GetProgram(document.GetIntegerValue("ProgramID", default(int)))?.CampaignID ?? 0;
            if (sku != null && document != null)
            {
                var product = mapper.Map<CampaignsProduct>(document);
                product.CampaignID = campaignId;
                return product;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<ProductCategory> GetProductCategories(int skuid)
        {
            var categories = new List<ProductCategory>();

            var skuCategories = SKUOptionCategoryInfoProvider
                .GetSKUOptionCategories()
                .Columns(nameof(SKUOptionCategoryInfo.CategoryID))
                .WhereEquals(nameof(SKUOptionCategoryInfo.SKUID), skuid)
                .OrderByDefault();

            foreach (var cat in skuCategories)
            {
                var category = OptionCategoryInfoProvider.GetOptionCategoryInfo(cat.CategoryID);
                if (category.CategoryType != OptionCategoryTypeEnum.Attribute
                    || !category.CategoryEnabled)
                {
                    continue;
                }
                var optionSkus = SKUInfoProvider.GetSKUOptionsForProduct(skuid, cat.CategoryID, true);

                var categoryModel = new ProductCategory
                {
                    Name = category.CategoryName,
                    Selector = category.CategorySelectionType.ToString(),
                    Options = GetCategoryOptions(category, optionSkus)
                };

                categories.Add(categoryModel);
            }

            return categories;
        }

        IList<ProductOption> GetCategoryOptions(OptionCategoryInfo category, IEnumerable<SKUInfo> skus)
        {
            var options = new List<ProductOption>();

            if (category.CategorySelectionType == OptionCategorySelectionTypeEnum.Dropdownlist)
            {
                options.Add(new ProductOption
                {
                    Name = category.CategoryDefaultRecord,
                    Disabled = true,
                    Selected = (category.CategoryDefaultRecord == category.CategoryDefaultOptions)
                });
            }

            foreach (var sku in skus)
            {
                options.Add(new ProductOption
                {
                    Name = sku.SKUName,
                    Id = sku.SKUID,
                    Selected = (sku.SKUID.ToString() == category.CategoryDefaultOptions),
                    Disabled = false
                });
            }

            return options;
        }

        public ProductPricingInfo GetDefaultVariantPricing(int documentId, string uomLocalized)
        {
            var document = DocumentHelper.GetDocument(documentId, new TreeProvider(MembershipContext.AuthenticatedUser));

            var skuCategories = SKUOptionCategoryInfoProvider
                .GetSKUOptionCategories()
                .Columns(nameof(SKUOptionCategoryInfo.CategoryID))
                .WhereEquals(nameof(SKUOptionCategoryInfo.SKUID), document.NodeSKUID);
            var optionCategories = OptionCategoryInfoProvider
                .GetOptionCategories()
                .Columns(nameof(OptionCategoryInfo.CategoryDefaultOptions))
                .WhereIn(nameof(OptionCategoryInfo.CategoryID), skuCategories)
                .And()
                .WhereEquals(nameof(OptionCategoryInfo.CategoryType), OptionCategoryTypeEnum.Attribute.ToStringRepresentation())
                .And()
                .WhereEquals(nameof(OptionCategoryInfo.CategoryEnabled), true)
                .ToList();

            SKUInfo variant = null;

            if (optionCategories.Count > 0)
            {
                variant = VariantHelper.GetProductVariant(document.NodeSKUID,
                    new ProductAttributeSet(optionCategories.Select(c =>
                    {
                        int.TryParse(c.CategoryDefaultOptions, out int id);
                        return id;
                    })));
            }

            var basePrice = variant?.SKUPrice ?? document.GetDoubleValue("SKUPrice", 0);

            var key = string.Format("{0} {1}",
                    ResHelper.GetString("Kadena.Product.BasePriceTitle", LocalizationContext.CurrentCulture.CultureCode), // 1+
                    uomLocalized);

            var value = string.Format("{0} {1}",
                    ResHelper.GetString("Kadena.Checkout.ItemPricePrefix", LocalizationContext.CurrentCulture.CultureCode), // $
                    basePrice.ToString("N2"));

            return new ProductPricingInfo
            {
                Id = "option-price",
                Key = key,
                Value = value
            };
        }

        public Dictionary<int, int> GetAllocatedProductQuantityForUser(int userID, List<int> campaignProductIds = null)
        {
            var query = CustomTableItemProvider.GetItems(CustomTableName)
                               .Columns("ProductID", "UserID", "Quantity");

            if (campaignProductIds?.Any() ?? false)
            {
                query = query.WhereIn("ProductID", campaignProductIds);
            }

            var allocatedItems = query
                               .Select(i => new
                               {
                                   ProductId = i.GetValue("ProductID", default(int)),
                                   UserId = i.GetValue("UserID", default(int)),
                                   Quantity = i.GetValue("Quantity", default(int))
                               })
                               .ToList();
            return allocatedItems
                .GroupBy(ai => ai.ProductId, (id, ai) => new
                {
                    ProductId = id,
                    Quantity = ai.FirstOrDefault(q => q.UserId == userID)?.Quantity ?? 0
                })
                .ToDictionary(i => i.ProductId, i => i.Quantity);
        }
    }
}
