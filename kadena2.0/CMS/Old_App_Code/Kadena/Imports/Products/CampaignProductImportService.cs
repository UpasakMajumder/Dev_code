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
using CMS.IO;
using CMS.DocumentEngine.Types.KDA;

using static Kadena.Helpers.SerializerConfig;
using CMS.CustomTables.Types.KDA;
using CMS.CustomTables;
using System.Net.Http;

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
            var productImages = GetDtosFromExcelRows<CampaignProductImageDto>(rows);
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
            if (string.IsNullOrEmpty(campaignProductDto.Campaign))
            {
                string inventoryProductsAliasPath = SettingsKeyInfoProvider.GetValue("KDA_InventoryProductFolderPath", siteID);
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
                parentDocument = GetProgram(campaignProductDto.Campaign, campaignProductDto.ProgramName);
            }
            return parentDocument;
        }

        private static void SetProductImage(CampaignProductImageDto image, int siteId)
        {
            var sku = GetUniqueSKU(image.SKU, siteId);
            var site = SiteInfoProvider.GetSiteInfo(siteId);
            var defaultSiteCulture = CultureHelper.GetDefaultCultureCode(site.SiteName);

            if (sku == null)
            {
                throw new Exception($"SKU with SKUNumber {image.SKU} doesn't exist");
            }

            var documents = DocumentHelper.GetDocuments(CampaignsProduct.CLASS_NAME)
                            .Path("/", PathTypeEnum.Children)
                            .WhereEquals("ClassName", CampaignsProduct.CLASS_NAME)
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
            RemoveTumbnail(product);
            AttachThumbnail(product, thumbnailUrl);
        }

        private static void RemoveProductImages(SKUTreeNode product)
        {
            product.RemoveImage();
            RemoveTumbnail(product);
        }

        private static void AttachThumbnail(SKUTreeNode product, string fromUrl)
        {
            var newAttachment = DownloadAttachmentThumbnail(product, fromUrl);
            if (newAttachment != null)
            {
                product.SetValue("ProductThumbnail", newAttachment.AttachmentGUID);
            }

        }

        private static AttachmentInfo DownloadAttachmentThumbnail(SKUTreeNode product, string fromUrl)
        {
            if (product.SKU == null)
            {
                throw new ArgumentNullException(nameof(product.SKU));
            }

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, fromUrl))
                {
                    using (var response = client.SendAsync(request).Result)
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var mimetype = response.Content?.Headers?.ContentType?.MediaType ?? string.Empty;

                            if (!mimetype.StartsWith("image/"))
                            {
                                throw new Exception("Thumbnail is not of image MIME type");
                            }

                            var stream = response.Content.ReadAsStreamAsync().Result;

                            var extension = Path.GetExtension(fromUrl);

                            if (string.IsNullOrEmpty(extension) && mimetype.StartsWith("image/"))
                            {
                                extension = mimetype.Split('/')[1];
                            }

                            // attach file as page attachment and set it's GUID as ProductThumbnail (of type guid) property of  Product
                            var newAttachment = new AttachmentInfo()
                            {
                                InputStream = stream,
                                AttachmentSiteID = product.NodeSiteID,
                                AttachmentDocumentID = product.DocumentID,
                                AttachmentExtension = extension,
                                AttachmentName = $"Thumbnail{product.SKU.GetStringValue("SKUProductCustomerReferenceNumber", product.SKU.SKUNumber)}.{extension}",
                                AttachmentLastModified = DateTime.Now,
                                AttachmentMimeType = mimetype,
                                AttachmentSize = (int)stream.Length
                            };

                            AttachmentInfoProvider.SetAttachmentInfo(newAttachment);

                            return newAttachment;
                        }
                        else
                        {
                            throw new Exception("Failed to download thumbnail image");
                        }
                    }
                }
            }
        }

        private static void RemoveTumbnail(SKUTreeNode product)
        {
            var oldAttachmentGuid = product.GetGuidValue("ProductThumbnail", Guid.Empty);
            var siteName = (product.Site ?? SiteInfoProvider.GetSiteInfo(product.NodeSiteID)).SiteName;
            if (oldAttachmentGuid != Guid.Empty)
            {
                var oldAttachment = AttachmentInfoProvider.GetAttachmentInfo(oldAttachmentGuid, siteName);
                if (oldAttachment != null)
                {
                    AttachmentInfoProvider.DeleteAttachmentInfo(oldAttachment);
                }
            }
        }

        private SKUTreeNode AppendProduct(TreeNode parent, CampaignProductDto product, SKUInfo sku, int siteId)
        {
            if (parent == null || product == null)
            {
                return null;
            }

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            SKUTreeNode existingProduct = (SKUTreeNode)parent.Children.FirstOrDefault(c => c.NodeSKUID == sku.SKUID);
            SKUTreeNode newProduct = existingProduct ?? (SKUTreeNode)TreeNode.New(CampaignsProduct.CLASS_NAME, tree);
            Program program = GetProgram(product.Campaign, product.ProgramName);
            ProductCategory productCategory = GetProductCategory(product.ProductCategory);

            newProduct.DocumentName = product.ProductName;
            newProduct.DocumentSKUName = product.ProductName;
            newProduct.NodeSKUID = sku.SKUID;
            newProduct.NodeName = product.ProductName;
            newProduct.DocumentCulture = _culture;
            newProduct.SetValue("ProductName", product.ProductName);
            newProduct.SetValue("BrandID", GetBrandID(product.Brand));
            SetProductItemSpecs(ref newProduct, product);
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

        private void SetProductItemSpecs(ref SKUTreeNode newProduct, CampaignProductDto product)
        {
            if (!string.IsNullOrWhiteSpace(product.ItemSpecs))
            {
                ProductItemSpecsItem itemSpecs = CustomTableItemProvider.GetItems<ProductItemSpecsItem>().WhereEquals("ItemSpec", product.ItemSpecs);
                if (itemSpecs != null)
                {
                    newProduct.SetValue("ItemSpecs", itemSpecs.ItemID);
                }
            }
            if (!string.IsNullOrWhiteSpace(product.CustomItemSpecs))
            {
                newProduct.SetValue("CustomItemSpecs", product.CustomItemSpecs);
            }
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
            CMS.DocumentEngine.Types.KDA.Campaign campaign = GetCampaign(campaignName);
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

        private CMS.DocumentEngine.Types.KDA.Campaign GetCampaign(string campaignName)
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
            if (string.IsNullOrWhiteSpace(sku))
            {
                return null;
            }
            var skus = SKUInfoProvider.GetSKUs(siteID)
                .WhereEquals("SKUNumber", sku);

            if (skus.Count() > 1)
            {
                throw new Exception($"Multiple SKUs with SKUNumber {sku} exist on site");
            }

            return skus.FirstObject;
        }

        private static SKUInfo GetUniqueSKUByPOSNumber(string pOSNumber, int siteID)
        {
            var skus = SKUInfoProvider.GetSKUs(siteID)
                .WhereEquals("SKUProductCustomerReferenceNumber", pOSNumber);

            if (skus.Count() > 1)
            {
                throw new Exception($"Multiple SKUs with POSNumber {pOSNumber} exist on site");
            }

            return skus.FirstObject;
        }

        private static SKUInfo EnsureSKU(CampaignProductDto product, int siteID)
        {
            var sku = GetUniqueSKUByPOSNumber(product.POSNumber, siteID) ?? new SKUInfo();

            sku.SKUName = product.ProductName;
            sku.SKUPrice = Convert.ToDouble(product.ActualPrice);
            sku.SKUSiteID = siteID;
            sku.SKUNumber = string.IsNullOrWhiteSpace(product.CenveoID) ? "00000" : product.CenveoID;
            sku.SKUDescription = product.LongDescription;
            if (product.Status.ToLower().Equals("active"))
            {
                sku.SKUEnabled = true;
            }
            else
            {
                sku.SKUEnabled = false;
            }

            if (string.IsNullOrWhiteSpace(product.Campaign))
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
            if (!string.IsNullOrWhiteSpace(product.ProductWeight))
            {
                sku.SKUWeight = ValidationHelper.GetDouble(product.ProductWeight, 0);
            }
            sku.SetValue("SKUProductCustomerReferenceNumber", product.POSNumber);

            SKUInfoProvider.SetSKUInfo(sku);
            return sku;
        }
    }
}