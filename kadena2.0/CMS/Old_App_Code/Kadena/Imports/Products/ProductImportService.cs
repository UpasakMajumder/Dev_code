using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine;
using Kadena.Models;
using Kadena.Models.Product;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DataEngine;
using CMS.SiteProvider;
using CMS.DocumentEngine.Types.KDA;

using static Kadena.Helpers.SerializerConfig;
using CMS.MediaLibrary;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public class ProductImportService : ImportServiceBase
    {
        private string _culture;

        public override ImportResult Process(byte[] importFileData, ExcelType type, int siteID)
        {
            CacheHelper.ClearCache();
            statusMessages.Clear();

            var site = GetSite(siteID);
            EnsureCulture(site);
            var rows = GetExcelRows(importFileData, type);
            var products = GetDtosFromExcelRows<ProductDto>(rows);

            var currentItemNumber = 0;
            foreach (var productDto in products)
            {
                currentItemNumber++;

                List<string> validationResults;
                if (!ValidateImportItem(productDto, out validationResults))
                {
                    statusMessages.Add($"Item number {currentItemNumber} has invalid values ({ string.Join("; ", validationResults) })");
                    continue;
                }

                try
                {
                    SaveProduct(productDto, siteID);
                }
                catch (Exception ex)
                {
                    statusMessages.Add($"There was an error when processing item #{currentItemNumber} : {ex.Message}");
                    EventLogProvider.LogException(typeof(ProductImportService).Name, "CREATEOBJ", ex);
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
                    EventLogProvider.LogException(typeof(ProductImportService).Name, "UPDATEOBJ", ex);
                }
            }

            CacheHelper.ClearCache();

            return new ImportResult
            {
                AllMessagesCount = statusMessages.AllMessagesCount,
                ErrorMessages = statusMessages.ToArray()
            };
        }

        protected ProductCategory CreateProductCategory(string[] path, int siteId)
        {
            var root = DocumentHelper.GetDocuments("KDA.ProductsModule")
                            .Path("/", PathTypeEnum.Children)
                            .WhereEquals("ClassName", "KDA.ProductsModule")
                            .Culture(_culture)
                            .CheckPermissions()
                            .NestingLevel(1)
                            .OnSite(new SiteInfoIdentifier(siteId))
                            .Published()
                            .FirstObject;

            return AppendProductCategory(root, path);
        }

        private static bool ValidateImportItem(ProductDto product, out List<string> validationErrors)
        {
            var errorMessageFormat = "field {0} - {1}";
            bool isValid = ValidatorHelper.ValidateDto(product, out validationErrors, errorMessageFormat);

            if (!isValid)
            {
                return false;
            }

            if (product.ProductType.Contains(ProductTypes.TemplatedProduct))
            {
                if (string.IsNullOrWhiteSpace(product.ChiliTemplateID) ||
                    string.IsNullOrWhiteSpace(product.ChiliWorkgroupID) ||
                    string.IsNullOrWhiteSpace(product.ChiliPdfGeneratorSettingsID))
                {

                    isValid = false;
                    validationErrors.Add("ChiliTemplateID, ChiliWorkgroupID and ChiliPdfGeneratorSettingsID are mandatory for Templated product");
                }
            }

            if (product.ProductType.Contains(ProductTypes.InventoryProduct) ||
                product.ProductType.Contains(ProductTypes.POD) ||
                product.ProductType.Contains(ProductTypes.StaticProduct) ||
                product.ProductType.Contains(ProductTypes.TemplatedProduct))
            {
                decimal weight = 0.0m;
                if (string.IsNullOrEmpty(product.PackageWeight) || !decimal.TryParse(product.PackageWeight, out weight))
                {
                    isValid = false;
                    validationErrors.Add($"{nameof(product.PackageWeight)} must be in numeric format");
                }
                if (weight <= 0.0m)
                {
                    isValid = false;
                    validationErrors.Add($"{nameof(product.PackageWeight)} must be > 0");
                }
            }


            if (!string.IsNullOrEmpty(product.PublishFrom) && !string.IsNullOrEmpty(product.PublishTo))
            {
                DateTime from, to;

                if (DateTime.TryParse(product.PublishFrom, out from) && DateTime.TryParse(product.PublishTo, out to))
                {
                    if (from > to)
                    {
                        isValid = false;
                        validationErrors.Add("If both are specified, PublishFrom must be earlier than PublishTo");
                    }
                }
                else
                {
                    isValid = false;
                    validationErrors.Add("PublishFrom and PublishTo must be in 'MM/dd/yyyy' format.");
                }
            }


            if (!string.IsNullOrEmpty(product.MinItemsInOrder) && !string.IsNullOrEmpty(product.MaxItemsInOrder))
            {
                uint min, max;
                if (uint.TryParse(product.MinItemsInOrder, out min) && uint.TryParse(product.MaxItemsInOrder, out max))
                {
                    if (min > max)
                    {
                        isValid = false;
                        validationErrors.Add("If both are specified, MinItemsInOrder must be less than MaxnItemsInOrder");
                    }
                }
                else
                {
                    isValid = false;
                    validationErrors.Add("MinItemsInOrder and MaxItemsInOrder must be non-negative integer.");
                }
            }

            if (Convert.ToInt32(product.ItemsInPackage) <= 0)
            {
                isValid = false;
                validationErrors.Add("ItemsInPackagemust be > 0");
            }

            return isValid;
        }

        private void SaveProduct(ProductDto productDto, int siteID)
        {
            var categories = productDto.ProductCategory.Split('\n');
            var productParent = CreateProductCategory(categories, siteID);
            var sku = EnsureSKU(productDto, siteID);
            AppendProduct(productParent, productDto, sku, siteID);
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

            var documents = DocumentHelper.GetDocuments("KDA.Product")
                            .Path("/", PathTypeEnum.Children)
                            .WhereEquals("ClassName", "KDA.Product")
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

            AssignImage(product, image.ImageMediaLibraryName, image.ImagePath);

            product.Update();
        }

        private static void AssignImage(SKUTreeNode product, string mediaLibraryName, string imagePath)
        {
            var library = MediaLibraryInfoProvider.GetMediaLibraryInfo(mediaLibraryName, SiteContext.CurrentSiteName);
            if (library == null)
            {
                throw new KeyNotFoundException($"Unable to assign image to SKU '{product.SKU.SKUNumber}'. Media library '{mediaLibraryName}' not found.");
            }

            var image = MediaFileInfoProvider.GetMediaFileInfo(library.LibraryID, imagePath);
            if (image == null)
            {
                throw new KeyNotFoundException($"Unable to assign image to SKU '{product.SKU.SKUNumber}'. File '{imagePath}' not found.");
            }

            product.RemoveImage();
            product.SetImage(image);
        }

        private SKUTreeNode AppendProduct(TreeNode parent, ProductDto product, SKUInfo sku, int siteId)
        {
            if (parent == null || product == null)
            {
                return null;
            }

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            SKUTreeNode existingProduct = (SKUTreeNode)parent.Children.FirstOrDefault(c => c.NodeSKUID == sku.SKUID);
            SKUTreeNode newProduct = existingProduct ?? (SKUTreeNode)TreeNode.New("KDA.Product", tree);

            newProduct.DocumentName = product.ProductName;
            newProduct.DocumentSKUName = product.ProductName;
            newProduct.NodeSKUID = sku.SKUID;
            newProduct.NodeName = product.ProductName;
            newProduct.DocumentCulture = _culture;
            newProduct.SetValue("ProductType", product.ProductType);
            newProduct.SetValue("ProductSKUWeight", Convert.ToDecimal(product.PackageWeight));
            newProduct.SetValue("ProductNumberOfItemsInPackage", Convert.ToInt32(product.ItemsInPackage));
            newProduct.SetValue("ProductChiliTemplateID", product.ChiliTemplateID ?? string.Empty);
            newProduct.SetValue("ProductChiliWorkgroupID", product.ChiliWorkgroupID ?? string.Empty);
            newProduct.SetValue("ProductChiliPdfGeneratorSettingsId", product.ChiliPdfGeneratorSettingsID ?? string.Empty);
            newProduct.SetValue("ProductSKUNeedsShipping", (product.NeedsShipping?.ToLower() ?? string.Empty) == "true");
            newProduct.SetValue("ProductChili3dEnabled", (product.Chili3DEnabled?.ToLower() ?? string.Empty) == "true");
            newProduct.SetValue("ProductDynamicPricing", GetDynamicPricingJson(product.DynamicPriceMinItems, product.DynamicPriceMaxItems, product.DynamicPrice));
            newProduct.SetValue("ProductCustomerReferenceNumber", product.CustomerReferenceNumber);
            newProduct.SetValue("ProductMachineType", product.MachineType);
            newProduct.SetValue("ProductColor", product.Color);
            newProduct.SetValue("ProductPaper", product.Paper);
            newProduct.SetValue("ProductProductionTime", product.ProductionTime);
            newProduct.SetValue("ProductShipTime", product.ShipTime);
            newProduct.SetValue("ProductShippingCost", product.ShippingCost);
            newProduct.SetValue("ProductSheetSize", product.SheetSize);
            newProduct.SetValue("ProductTrimSize", product.TrimSize);
            newProduct.SetValue("ProductFinishedSize", product.FinishedSize);
            newProduct.SetValue("ProductBindery", product.Bindery);

            if (DateTime.TryParse(product.PublishFrom, out DateTime publishFrom))
            {
                newProduct.DocumentPublishFrom = publishFrom;
            }
            if (DateTime.TryParse(product.PublishTo, out DateTime publishTo))
            {
                newProduct.DocumentPublishTo = publishTo;
            }

            SetPageTemplate(newProduct, "_Kadena_Product_Detail");

            if (existingProduct == null)
            {
                newProduct.Insert(parent);
            }

            if (!string.IsNullOrEmpty(product.ImageMediaLibraryName) && !string.IsNullOrEmpty(product.ImagePath))
            {
                AssignImage(newProduct, product.ImageMediaLibraryName, product.ImagePath);
            }
            else
            {
                newProduct.RemoveImage();
            }

            newProduct.Update();

            return newProduct;
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

        private ProductCategory AppendProductCategory(TreeNode parentPage, string[] subnodes)
        {
            if (parentPage == null || subnodes == null || subnodes.Length <= 0)
            {
                return ProductCategoryProvider
                    .GetProductCategories()
                    .Culture(_culture)
                    .WhereEquals(nameof(parentPage.DocumentID), parentPage.DocumentID)
                    .FirstObject;
            }

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            TreeNode category = parentPage.Children.FirstOrDefault(c => c.NodeName == subnodes[0]);

            if (category == null)
            {
                category = TreeNode.New("KDA.ProductCategory", tree);
                category.DocumentName = subnodes[0];
                category.DocumentCulture = _culture;
                SetPageTemplate(category, "_KDA_ProductCategory");
                category.Insert(parentPage);
            }

            return AppendProductCategory(category, subnodes.Skip(1).ToArray());
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

        private static SKUInfo EnsureSKU(ProductDto product, int siteID)
        {
            var sku = GetUniqueSKU(product.SKU, siteID) ?? new SKUInfo();

            sku.SKUName = product.ProductName;
            sku.SKUPrice = Convert.ToDouble(product.Price);
            sku.SKUEnabled = true;
            sku.SKUSiteID = siteID;
            sku.SKUNumber = product.SKU;
            sku.SKUDescription = product.Description;
            sku.SKUTrackInventory = ParseTrackInventoryTypeEnum(product.TrackInventory);

            if (!string.IsNullOrWhiteSpace(product.MinItemsInOrder))
            {
                sku.SetValue("SKUMinItemsInOrder", Convert.ToInt32(product.MinItemsInOrder));
            }

            if (!string.IsNullOrWhiteSpace(product.MaxItemsInOrder))
            {
                sku.SetValue("SKUMaxItemsInOrder", Convert.ToInt32(product.MaxItemsInOrder));
            }

            if (!string.IsNullOrEmpty(product.SellOnlyIfItemsAvailable))
            {
                sku.SetValue("SKUSellOnlyAvailable", product.SellOnlyIfItemsAvailable.ToLower() == "true");
            }

            SKUInfoProvider.SetSKUInfo(sku);
            return sku;
        }
    }
}