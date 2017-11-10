using CMS.DocumentEngine;
using CMS.Ecommerce;
using CMS.EventLog;
using CMS.Helpers;
using CMS.Localization;
using CMS.MediaLibrary;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.SiteProvider;
using Kadena.Models;
using Kadena.Models.Product;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

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
            statusMessages.Clear();

            var site = GetSite(siteID);
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

        private bool ValidateImportItem(ProductDto product, out List<string> validationErrors)
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
                if(string.IsNullOrEmpty(product.PackageWeight) ||  !decimal.TryParse(product.PackageWeight, out weight))
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

        private void SetProductImage(ProductImageDto image, int siteId)
        {
            var skus = SKUInfoProvider.GetSKUs(siteId)
               .WhereEquals("SKUNumber", image.SKU);

            if (skus.Count() > 1)
            {
                throw new Exception("More than .... TODO after merge"); // TODO
            }

            var sku = skus.FirstObject;

            if (sku == null)
            {
                throw new Exception($"SKU with SKUNumber {image.SKU} doesn't exist");
            }

            var documents = DocumentHelper.GetDocuments("KDA.Product")
                            .Path("/", PathTypeEnum.Children)
                            .WhereEquals("ClassName", "KDA.Product")
                            .WhereEquals("NodeSKUID", sku.SKUID)
                            .Culture(LocalizationContext.CurrentCulture.CultureCode)
                            .CheckPermissions()
                            .OnSite(new CMS.DataEngine.SiteInfoIdentifier(siteId))
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

            SetProductImage(image, product, sku, siteId);
        }

        private void SetProductImage(ProductImageDto image, SKUTreeNode product, SKUInfo sku, int siteId)
        {
            string libraryImageUrl = DownloadImageToMedialibrary(image.ImageURL);

            SetProductImage(product, libraryImageUrl);

            var newAttachment = DownloadAndAttachImage(image.ThumbnailURL, sku.SKUNumber, product.DocumentID, siteId);

            if (newAttachment != null)
            {
                product.SetValue("ProductThumbnail", newAttachment.AttachmentGUID); // todo check not to save twice
                product.Update();
            }
        }

        private AttachmentInfo DownloadAndAttachImage(string url, string skuNumber, int documentId, int siteId)
        {
            AttachmentInfo newAttachment = null;

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {

                    var response = client.SendAsync(request).Result;

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var stream = response.Content.ReadAsStreamAsync().Result;

                        // attach file as page attachment and set it's GUID as ProductThumbnail (of type guid) property of  Product
                        newAttachment = new AttachmentInfo()
                        {
                            InputStream = stream,
                            AttachmentSiteID = siteId,
                            AttachmentDocumentID = documentId,
                            AttachmentExtension = ".png", // TODO dehardcode extension
                            AttachmentName = $"Thumbnail{skuNumber}.png",
                            AttachmentLastModified = DateTime.Now,
                            AttachmentMimeType = "image/png",
                            AttachmentSize = (int)stream.Length
                        };

                    }
                }
            }

            if (newAttachment != null)
            {
                AttachmentInfoProvider.SetAttachmentInfo(newAttachment);
            }

            return newAttachment;
        }

        private AttachmentInfo DownloadImageToMedialibrary(string url, string skuNumber, int documentId, int siteId)
        {
            var library = EnsureLibrary(siteId);
            /*
            AttachmentInfo newAttachment = null;

            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, url))
                {

                    var response = client.SendAsync(request).Result;

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var stream = response.Content.ReadAsStreamAsync().Result;

                        

                        // Prepares a path to a local file
                        string filePath = @"C:\Files\images\Image.png";

                        // Prepares a CMS.IO.FileInfo object representing the local file
                        CMS.IO.FileInfo file = CMS.IO.FileInfo.New();

                        if (file != null)
                        {
                            // Creates a new media library file object
                            MediaFileInfo mediaFile = new MediaFileInfo(filePath, library.LibraryID);

                            // Sets the media library file properties
                            mediaFile.FileName = "Image";
                            mediaFile.FileTitle = "File title";
                            mediaFile.FileDescription = "This file was added through the API.";
                            mediaFile.FilePath = "NewFolder/Image/"; // Sets the path within the media library's folder structure
                            mediaFile.FileExtension = file.Extension;
                            mediaFile.FileMimeType = MimeTypeHelper.GetMimetype(file.Extension);
                            mediaFile.FileSiteID = SiteContext.CurrentSiteID;
                            mediaFile.FileLibraryID = library.LibraryID;
                            mediaFile.FileSize = file.Length;

                            

                            // Saves the media library file
                            MediaFileInfoProvider.SetMediaFileInfo(mediaFile);
                        }

                    }
                }
            }
            
            if (newAttachment != null)
            {
                AttachmentInfoProvider.SetAttachmentInfo(newAttachment);
            }

            return newAttachment;
            */

            return null;
        }

        private MediaLibraryInfo EnsureLibrary(int siteId)
        {
            string libraryName = "ProductImages";
            var siteName = SiteInfoProvider.GetSiteInfo(siteId).SiteName;
            var meidaLibrary = MediaLibraryInfoProvider.GetMediaLibraryInfo(libraryName, siteName);
            if (meidaLibrary == null)
            {
                // Creates a new media library object
                meidaLibrary = new MediaLibraryInfo();

                // Sets the library properties
                meidaLibrary.LibraryDisplayName = libraryName;
                meidaLibrary.LibraryName = libraryName;
                meidaLibrary.LibraryDescription = "Media library for storing product images";
                meidaLibrary.LibraryFolder = "Products";
                meidaLibrary.LibrarySiteID = SiteContext.CurrentSiteID;

                // Saves the new media library to the database
                MediaLibraryInfoProvider.SetMediaLibraryInfo(meidaLibrary);
            }

            return meidaLibrary;
        }

        /// <summary>
        /// Sets given <param name="imageUrl"></param> as SKUImagePath of product node
        /// </summary>
        private void SetProductImage(SKUTreeNode product, string imageUrl)
        {
            product.SetValue("SKUImagePath", imageUrl);
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

            if (ranges.Any(r => r.MaxVal < r.MinVal))
            {
                throw new ArgumentOutOfRangeException("DynamicPriceMinItems,DynamicPriceMaxItems", "All Dynamic Pricing definition ranges must have Min <= Max.");
            }

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
            TreeNode category = parentPage.Children.FirstOrDefault(c => c.NodeName == subnodes[0]);

            if (category == null)
            {
                category = TreeNode.New("KDA.ProductCategory", tree);
                category.DocumentName = subnodes[0];
                category.DocumentCulture = "en-us";

                // To set category image:
                // category.SetValue("ProductCategoryImage", $"https://dummyimage.com/320/0000ff/ffffff.png&text={subnodes[0]}");

                SetPageTemplate(category, "_KDA_ProductCategory");
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
            var skus = SKUInfoProvider.GetSKUs(siteID)
                .WhereEquals("SKUNumber", product.SKU);

            if (skus.Count() > 1)
            {
                throw new Exception($"Multiple SKUs with SKUNumber {product.SKU} exist on site");
            }

            var sku = skus.FirstObject ?? new SKUInfo();            

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