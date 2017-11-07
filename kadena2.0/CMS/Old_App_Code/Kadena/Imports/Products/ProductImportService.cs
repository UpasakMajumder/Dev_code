using CMS.Base.Web.UI;
using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Localization;
using CMS.Membership;
using CMS.PortalEngine;
using Kadena.Models;
using Kadena.Models.Product;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public class ProductImportService : ImportServiceBase
    {
        protected static JsonSerializerSettings camelCaseSerializer = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        public ImportResult ProcessProductsImportFile(byte[] importFileData, ExcelType type, int siteID)
        {
            CacheHelper.ClearCache();

            var site = GetSite(siteID);
            var rows = GetExcelRows(importFileData, type);
            var products = GetDtosFromExcelRows<ProductDto>(rows);
            var statusMessages = new List<string>();

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
                    EventLogProvider.LogException("Import products", "EXCEPTION", ex);
                }
            }

            CacheHelper.ClearCache();

            return new ImportResult
            {
                ErrorMessages = statusMessages.ToArray()
            };
        }

        public ImportResult ProcessProductImagesImportFile(byte[] importFileData, ExcelType type, int siteID)
        {
            CacheHelper.ClearCache();

            var site = GetSite(siteID);
            var rows = GetExcelRows(importFileData, type);
            var productImages = GetDtosFromExcelRows<ProductImageDto>(rows);
            var statusMessages = new List<string>();

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
                    //SetProductImage(imageDto, siteID);
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
                ErrorMessages = statusMessages.ToArray()
            };
        }

        private bool ValidateImportItem(ProductDto product, out List<string> validationErrors)
        {
            var errorMessageFormat = "field {0} - {1}";
            bool isValid = ValidatorHelper.ValidateDto(product, out validationErrors, errorMessageFormat);

            // validate special rules
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

            return isValid;
        }

        private void SaveProduct(ProductDto productDto, int siteID)
        {
            var categories = productDto.ProductCategory.Split('\n');
            var productParent = CreateProductCategory(categories, siteID);
            var sku = EnsureSKU(productDto, siteID);
            var newProduct = AppendProduct(productParent, productDto, sku, siteID);
            SetProductImage(null, newProduct, sku, siteID);
        }

        private void SetProductImage(ProductImageDto image, SKUTreeNode product, SKUInfo sku, int siteId)
        {
            product.SetValue("SKUImagePath", $"https://dummyimage.com/320/0000ff/ffffff.png&text={sku.SKUName} SKUImagePath");

            var newAttachment = new AttachmentInfo(@"C:\doc\thumbnail.png")
            {
                AttachmentSiteID = siteId,
                AttachmentDocumentID = product.DocumentID
            };

            AttachmentInfoProvider.SetAttachmentInfo(newAttachment);
            product.SetValue("ProductThumbnail", newAttachment.AttachmentGUID); // todo check not to save twice
            product.Update();
        }

        private SKUTreeNode AppendProduct(TreeNode parent, ProductDto product, SKUInfo sku, int siteId)
        {
            if (parent == null || product == null)
                return null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            SKUTreeNode existingProduct = (SKUTreeNode)parent.Children.FirstOrDefault(c => c.NodeSKUID == sku.SKUID);
            SKUTreeNode newProduct = existingProduct ?? (SKUTreeNode)TreeNode.New("KDA.Product", tree);

            newProduct.DocumentName = product.ProductName;
            newProduct.DocumentSKUName = product.ProductName;
            newProduct.NodeSKUID = sku.SKUID;
            newProduct.NodeName = product.ProductName;
            newProduct.DocumentCulture = LocalizationContext.PreferredCultureCode;
            newProduct.SetValue("ProductType", product.ProductType);
            newProduct.SetValue("ProductSKUWeight", Convert.ToDecimal(product.PackageWeight));
            newProduct.SetValue("ProductNumberOfItemsInPackage", Convert.ToInt32(product.ItemsInPackage));
            newProduct.SetValue("ProductChiliTemplateID", product.ChiliTemplateID ?? string.Empty);
            newProduct.SetValue("ProductChiliWorkgroupID", product.ChiliWorkgroupID ?? string.Empty);
            newProduct.SetValue("ProductChiliPdfGeneratorSettingsId", product.ChiliPdfGeneratorSettingsID ?? string.Empty);
            newProduct.SetValue("ProductSKUNeedsShipping", product.NeedsShipping.ToLower() == "true");
            newProduct.SetValue("ProductChili3dEnabled", product.Chili3DEnabled.ToLower() == "true");
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
            
            DateTime publishFrom, publishTo;
            if (DateTime.TryParse(product.PublishFrom, out publishFrom))
            {
                newProduct.DocumentPublishFrom = publishFrom;
            }
            if (DateTime.TryParse(product.PublishTo, out publishTo))
            {
                newProduct.DocumentPublishTo = publishTo;
            }

            SetPageTemplate(newProduct, "_Kadena_Product_Detail");

            if (existingProduct == null) // todo check not to save twice with storing image
            {
                newProduct.Insert(parent);
            }
            else
            {
                newProduct.Update();
            }

            return newProduct;
        }

        protected void SetPageTemplate(TreeNode node, string templateName)
        {
            var pageTemplateInfo = PageTemplateInfoProvider.GetPageTemplateInfo(templateName);
            node.DocumentPageTemplateID = pageTemplateInfo?.PageTemplateId ?? 0;
            node.NodeTemplateForAllCultures = true;
        }


        private string GetDynamicPricingJson(string min, string max, string price)
        {
            int[] mins, maxes;
            decimal[] prices;

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
                throw new ArgumentOutOfRangeException("Bad format of Dynamic Pricing definitions.", ex);
            }


            if (mins.Length != maxes.Length || mins.Length != prices.Length)
            {
                throw new ArgumentOutOfRangeException("Dynamic Pricing definition cells must contain the same count of rows in one product.");
            }


            var ranges = mins.Select((item, index) => new DynamicPricingRange { MinVal = item, MaxVal = maxes[index], Price = prices[index] }).ToList();

            return JsonConvert.SerializeObject(ranges, camelCaseSerializer);
        }

        private TreeNode CreateProductCategory(string[] path, int siteId)
        {
            var root = DocumentHelper.GetDocuments("KDA.ProductsModule")
                            .Path("/", PathTypeEnum.Children)
                            .WhereEquals("ClassName", "KDA.ProductsModule")
                            .Culture(LocalizationContext.CurrentCulture.CultureCode)
                            .CheckPermissions()
                            .NestingLevel(1)
                            .OnSite(new CMS.DataEngine.SiteInfoIdentifier(siteId))
                            .Published()
                            .FirstObject;

            return AppendProductCategory(root, path);
        }

        private TreeNode AppendProductCategory(TreeNode parentPage, string[] subnodes)
        {
            if (parentPage == null || subnodes == null || subnodes.Length <= 0)
                return parentPage;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);

            //try to find existing category
            TreeNode category = parentPage.Children.FirstOrDefault(c => c.NodeName == subnodes[0]);

            if (category == null)
            {
                category = TreeNode.New("KDA.ProductCategory", tree);
                category.DocumentName = subnodes[0];
                category.DocumentCulture = "en-us";
                category.SetValue("ProductCategoryImage", $"https://dummyimage.com/320/0000ff/ffffff.png&text={subnodes[0]}");

                SetPageTemplate(category, "_KDA_ProductCategory");

                // Inserts the new page as a child of the parent page
                category.Insert(parentPage);
            }

            return AppendProductCategory(category, subnodes.Skip(1).ToArray());
        }

        private TrackInventoryTypeEnum ParseTrackInventoryTypeEnum(string value)
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

        private SKUInfo GetSKU(string sku, int siteID)
        {
            return SKUInfoProvider.GetSKUs()
                .WhereEquals("SKUNumber", sku)
                .FirstObject;
        }

        private SKUInfo EnsureSKU(ProductDto product, int siteID)
        {
            var sku = GetSKU(product.SKU, siteID ) ?? new SKUInfo();            

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

            sku.SetValue("SKUSellOnlyAvailable", product.SellOnlyIfItemsAvailable.ToLower() == "true");

            SKUInfoProvider.SetSKUInfo(sku);
            return sku;
        }
    }
}