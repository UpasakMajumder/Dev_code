using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine;
using Kadena.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;
using CMS.SiteProvider;
using CMS.DocumentEngine.Types.KDA;

using static Kadena.Helpers.SerializerConfig;
using CMS.CustomTables.Types.KDA;
using CMS.CustomTables;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public class CampaignProductImportService : ImportServiceBase
    {
        private string _culture;
        private int _site;

        public override ImportResult Process(byte[] importFileData, ExcelType type, int siteID)
        {
            CacheHelper.ClearCache();
            statusMessages.Clear();

            var site = GetSite(siteID);
            _site = siteID;
            EnsureCulture(site);
            var rows = GetExcelRows(importFileData, type);
            var products = GetDtosFromExcelRows<CampaignProductDto>(rows);

            var currentItemNumber = 0;
            foreach (var CampaignProductDto in products)
            {
                currentItemNumber++;

                List<string> validationResults;
                if (!ValidateImportItem(CampaignProductDto, out validationResults))
                {
                    statusMessages.Add($"Item number {currentItemNumber} has invalid values ({ string.Join("; ", validationResults) })");
                    continue;
                }

                try
                {
                    SaveProduct(CampaignProductDto, siteID);
                }
                catch (Exception ex)
                {
                    statusMessages.Add($"There was an error when processing item #{currentItemNumber} : {ex.Message}");
                    EventLogProvider.LogException("Import products", "EXCEPTION", ex);
                }
            }

            CacheHelper.ClearCache();

            return new ImportResult
            {
                AllMessagesCount = statusMessages.AllMessagesCount,
                ErrorMessages = statusMessages.ToArray()
            };
        }

        protected void EnsureCulture(SiteInfo site)
        {
            _culture = SettingsKeyInfoProvider.GetValue($"{site.SiteName}.CMSDefaultCultureCode");
        }

        public ImportResult ProcessProductImagesImportFile(byte[] importFileData, ExcelType type, int siteID)
        {
            CacheHelper.ClearCache();

            var site = GetSite(siteID);
            EnsureCulture(site);
            var rows = GetExcelRows(importFileData, type);
            var productImages = GetDtosFromExcelRows<ProductImageDto>(rows);
            statusMessages.Clear();

            var currentItemNumber = 0;
            foreach (var imageDto in productImages)
            {
                currentItemNumber++;

                List<string> validationResults;
                if (!ValidatorHelper.ValidateDto(imageDto, out validationResults, "{0} - {1}"))
                {
                    statusMessages.Add($"Item number {currentItemNumber} has invalid values ({ string.Join("; ", validationResults) })");
                    continue;
                }

                try
                {
                    SetProductImage(imageDto, siteID);
                }
                catch (Exception ex)
                {
                    statusMessages.Add($"There was an error when processing item #{currentItemNumber} : {ex.Message}");
                    EventLogProvider.LogException("Import product images", "EXCEPTION", ex);
                }
            }

            CacheHelper.ClearCache();

            return new ImportResult
            {
                AllMessagesCount = statusMessages.AllMessagesCount,
                ErrorMessages = statusMessages.ToArray()
            };
        }

        private static bool ValidateImportItem(CampaignProductDto product, out List<string> validationErrors)
        {
            var errorMessageFormat = "field {0} - {1}";
            bool isValid = ValidatorHelper.ValidateDto(product, out validationErrors, errorMessageFormat);

            if (!isValid)
            {
                return false;
            }

            return isValid;
        }

        private void SaveProduct(CampaignProductDto campaignProductDto, int siteID)
        {
            TreeNode productParent = GetProductParent(campaignProductDto, siteID);
            SKUInfo sku = EnsureSKU(campaignProductDto, siteID);
            AppendProduct(productParent, campaignProductDto, sku, siteID);
        }

        private TreeNode GetProductParent(CampaignProductDto campaignProductDto, int siteID)
        {
            TreeNode parentDocument = null;
            SiteInfo site = SiteInfoProvider.GetSiteInfo(siteID);
            if (string.IsNullOrEmpty(campaignProductDto.Campagin))
            {
                string inventoryProductsAliasPath = SettingsKeyInfoProvider.GetValue("KDA_InventoryProductPath", siteID);
                parentDocument = DocumentHelper.GetDocument(
                    new NodeSelectionParameters
                    {
                        AliasPath = inventoryProductsAliasPath,
                        SiteName = site.SiteName,
                        CultureCode = site.DefaultVisitorCulture,
                        CombineWithDefaultCulture = false
                    },
                    new TreeProvider(MembershipContext.AuthenticatedUser)
                );
            }
            else
            {
                parentDocument = GetProgram(campaignProductDto.Campagin, campaignProductDto.ProgramName);
            }
            return parentDocument;
        }

        private static void SetProductImage(ProductImageDto image, int siteId)
        {
            var sku = GetUniqueSKU(image.SKU, siteId);
            var site = SiteInfoProvider.GetSiteInfo(siteId);
            var defaultSiteCulture = CultureHelper.GetDefaultCultureCode(site.SiteName);

            if (sku == null)
            {
                throw new Exception($"SKU with SKUNumber {image.SKU} doesn't exist");
            }

            var documents = DocumentHelper.GetDocuments("KDA.CampaignsProduct")
                            .Path("/", PathTypeEnum.Children)
                            .WhereEquals("ClassName", "KDA.CampaignsProduct")
                            .WhereEquals("NodeSKUID", sku.SKUID)
                            .Culture(defaultSiteCulture)
                            .CheckPermissions()
                            .OnSite(new SiteInfoIdentifier(siteId))
                            .Published();

            if (documents.Count() > 1)
            {
                throw new Exception($"Multiple product assigned to SKU with SKUNumber {image.SKU}");
            }

            var product = documents.FirstObject as SKUTreeNode;

            if (product == null)
            {
                throw new Exception($"No product assigned to SKU with SKUNumber {image.SKU}");
            }

            GetAndSaveProductImages(product, image.ImageURL, image.ThumbnailURL);

            product.Update();
        }

        // ready for potential use in Product upload
        private static void GetAndSaveProductImages(SKUTreeNode product, string imageUrl, string thumbnailUrl)
        {
            var library = new MediaLibrary
            {
                SiteId = product.NodeSiteID,
                LibraryName = "ProductImages",
                LibraryFolder = "Products",
                LibraryDescription = "Media library for storing product images"
            };
            var libraryImageUrl = library.DownloadImageToMedialibrary(imageUrl, $"Image{product.SKU.SKUNumber}", $"Product image for SKU {product.SKU.SKUNumber}");

            product.RemoveImage();
            product.SetImage(libraryImageUrl);
            product.RemoveTumbnail();
            product.AttachThumbnail(thumbnailUrl);
        }

        private static void RemoveProductImages(SKUTreeNode product)
        {
            product.RemoveImage();
            product.RemoveTumbnail();
        }

        private SKUTreeNode AppendProduct(TreeNode parent, CampaignProductDto product, SKUInfo sku, int siteId)
        {
            if (parent == null || product == null)
            {
                return null;
            }

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            SKUTreeNode existingProduct = (SKUTreeNode)parent.Children.FirstOrDefault(c => c.NodeSKUID == sku.SKUID);
            SKUTreeNode newProduct = existingProduct ?? (SKUTreeNode)TreeNode.New("KDA.CampaignsProduct", tree);
            Program program = GetProgram(product.Campagin, product.ProgramName);
            ProductCategory productCategory = GetProductCategory(product.ProductCategory);

            newProduct.DocumentName = product.ProductName;
            newProduct.DocumentSKUName = product.ProductName;
            newProduct.NodeSKUID = sku.SKUID;
            newProduct.NodeName = product.ProductName;
            newProduct.DocumentCulture = _culture;
            newProduct.SetValue("ProductName", product.ProductName);
            newProduct.SetValue("BrandID", GetBrandID(product.Brand));
            if (program != null)
            {
                newProduct.SetValue("ProgramID", program.ProgramID);
            }
            if (!string.IsNullOrWhiteSpace(product.AllowedStates))
            {
                newProduct.SetValue("State", GetStatesGroupID(product.AllowedStates));
            }
            if (!string.IsNullOrWhiteSpace(product.EstimatedPrice))
            {
                newProduct.SetValue("EstimatedPrice", product.EstimatedPrice);
            }
            if (productCategory != null)
            {
                newProduct.SetValue("CategoryID", productCategory.ProductCategoryID);
            }
            if (!string.IsNullOrWhiteSpace(product.BundleQuantity))
            {
                newProduct.SetValue("QtyPerPack", product.BundleQuantity);
            }
            if (!string.IsNullOrWhiteSpace(product.CenveoID))
            {
                newProduct.SetValue("CVOProductID", product.CenveoID);
            }

            if (existingProduct == null)
            {
                newProduct.Insert(parent);
            }

            if (!string.IsNullOrEmpty(product.ImageURL) && !string.IsNullOrEmpty(product.ThumbnailURL))
            {
                GetAndSaveProductImages(newProduct, product.ImageURL, product.ThumbnailURL);
            }
            else
            {
                RemoveProductImages(newProduct);
            }

            newProduct.Update();

            return newProduct;
        }

        private int GetStatesGroupID(string statesGroupName)
        {
            int statesGroupID = default(int);
            if (!string.IsNullOrWhiteSpace(statesGroupName))
            {
                StatesGroupItem statesGroup = CustomTableItemProvider.GetItems<StatesGroupItem>().WhereEquals("GroupName", statesGroupName).FirstOrDefault();
                if (statesGroup != null)
                {
                    statesGroupID = statesGroup.ItemID;
                }
            }
            return statesGroupID;
        }

        private int GetBrandID(string brandName)
        {
            int brandID = default(int);
            if (!string.IsNullOrWhiteSpace(brandName))
            {
                BrandItem brand = CustomTableItemProvider.GetItems<BrandItem>().WhereEquals("BrandName", brandName).FirstOrDefault();
                if (brand != null)
                {
                    brandID = brand.ItemID;
                }
            }
            return brandID;
        }

        private ProductCategory GetProductCategory(string productCategoryName)
        {
            if (string.IsNullOrWhiteSpace(productCategoryName))
            {
                return null;
            }
            else
            {
                return ProductCategoryProvider.GetProductCategories()
                                  .OnSite(_site)
                                  .Where(x => x.ProductCategoryTitle.Equals(productCategoryName) || x.DocumentName.Equals(productCategoryName))
                                  .FirstOrDefault();
            }
        }

        private Program GetProgram(string campaignName, string programName)
        {
            if (string.IsNullOrWhiteSpace(campaignName) || string.IsNullOrWhiteSpace(programName))
            {
                return null;
            }
            Campaign campaign = GetCampaign(campaignName);
            if (campaign != null)
            {
                return ProgramProvider.GetPrograms()
                                      .OnSite(_site)
                                      .Where(x => x.NodeParentID.Equals(campaign.NodeID) && x.ProgramName.Equals(programName))
                                      .FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        private Campaign GetCampaign(string campaignName)
        {
            return CampaignProvider.GetCampaigns()
                                   .OnSite(_site)
                                   .Where(x => x.DocumentName.Equals(campaignName))
                                   .FirstOrDefault();
        }

        private static void SetPageTemplate(TreeNode node, string templateName)
        {
            var pageTemplateInfo = PageTemplateInfoProvider.GetPageTemplateInfo(templateName);
            node.DocumentPageTemplateID = pageTemplateInfo?.PageTemplateId ?? 0;
            node.NodeTemplateForAllCultures = true;
        }

        private static string GetDynamicPricingJson(string min, string max, string price)
        {
            int[] mins, maxes;
            decimal[] prices;

            var message = "Bad format of Dynamic Pricing definitions. DynamicPriceMinItems, DynamicPriceMaxItems and DynamicPrice cells must contain the same count of rows in one product in proper numeric format.";

            if (string.IsNullOrWhiteSpace(min) && string.IsNullOrWhiteSpace(max) && string.IsNullOrWhiteSpace(price))
            {
                return string.Empty;
            }

            try
            {
                mins = min.Split('\n').Select(m => Convert.ToInt32(m)).ToArray();
                maxes = max.Split('\n').Select(m => Convert.ToInt32(m)).ToArray();
                prices = price.Split('\n').Select(m => Convert.ToDecimal(m)).ToArray();
            }
            catch (Exception ex)
            {
                throw new ArgumentOutOfRangeException(message, ex);
            }

            if (mins.Length != maxes.Length || mins.Length != prices.Length)
            {
                throw new ArgumentOutOfRangeException(message);
            }

            var ranges = mins.Select((item, index) => new DynamicPricingRange { MinVal = item, MaxVal = maxes[index], Price = prices[index] }).ToList();

            var dynamicRangeErrors = new List<string>();
            if (!DynamicPricingRange.ValidateRanges(ranges, dynamicRangeErrors))
            {
                var allMessages = string.Join(Environment.NewLine, dynamicRangeErrors);
                throw new ArgumentOutOfRangeException("DynamicPriceMinItems,DynamicPriceMaxItems", allMessages);
            }

            if (ranges.Any(r => r.MaxVal < r.MinVal))
            {
                throw new ArgumentOutOfRangeException("DynamicPriceMinItems,DynamicPriceMaxItems", "All Dynamic Pricing definition ranges must have Min <= Max.");
            }

            return JsonConvert.SerializeObject(ranges, CamelCaseSerializer);
        }

        private static TrackInventoryTypeEnum ParseTrackInventoryTypeEnum(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value == "Yes")
                    return TrackInventoryTypeEnum.ByProduct;
                else if (value == "By variants")
                    return TrackInventoryTypeEnum.ByVariants;
            }
            return TrackInventoryTypeEnum.Disabled;
        }

        private static SKUInfo GetUniqueSKU(string sku, int siteID)
        {
            var skus = SKUInfoProvider.GetSKUs(siteID)
                .WhereEquals("SKUNumber", sku);

            if (skus.Count() > 1)
            {
                throw new Exception($"Multiple SKUs with SKUNumber {sku} exist on site");
            }

            return skus.FirstObject;
        }

        private static SKUInfo EnsureSKU(CampaignProductDto product, int siteID)
        {
            var sku = GetUniqueSKU(product.SKU, siteID) ?? new SKUInfo();

            sku.SKUName = product.ProductName;
            sku.SKUPrice = Convert.ToDouble(product.ActualPrice);
            sku.SKUSiteID = siteID;
            sku.SKUNumber = product.SKU;
            sku.SKUDescription = product.LongDescription;
            if (product.Status.ToLower().Equals("active"))
            {
                sku.SKUEnabled = true;
            }
            else
            {
                sku.SKUEnabled = false;
            }

            if (string.IsNullOrWhiteSpace(product.Campagin))
            {
                sku.SKUTrackInventory = TrackInventoryTypeEnum.ByProduct;
            }
            if (!string.IsNullOrWhiteSpace(product.SKUValidUntil))
            {
                sku.SKUValidUntil = ValidationHelper.GetDateTime(product.SKUValidUntil, DateTime.Now);
            }
            if (!string.IsNullOrWhiteSpace(product.TotalQuantity))
            {
                sku.SKUAvailableItems = ValidationHelper.GetInteger(product.TotalQuantity, 0);
            }

            SKUInfoProvider.SetSKUInfo(sku);
            return sku;
        }
    }
}