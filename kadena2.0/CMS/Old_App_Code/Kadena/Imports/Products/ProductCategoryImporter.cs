using CMS.EventLog;
using CMS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Types.KDA;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public sealed class ProductCategoryImporter : ProductImportService
    {
        public override ImportResult Process(byte[] importFileData, ExcelType type, int siteId)
        {
            CacheHelper.ClearCache();

            var site = GetSite(siteId);
            EnsureCulture(site);

            var rows = GetExcelRows(importFileData, type);
            var productCategories = GetDtosFromExcelRows<ProductCategoryImportDto>(rows)
                .Select(pc => new
                {
                    ProductCategory = pc.ProductCategory.Split('\n'),
                    pc.ImageURL,
                    pc.Description
                })
                .OrderBy(pc => pc.ProductCategory.Count())
                .ToList();
            statusMessages.Clear();

            for (int i = 0; i < productCategories.Count(); i++)
            {
                var productCategory = productCategories[i];
                if (!ValidatorHelper.ValidateDto(productCategory, out List<string> validationResults, "{0} - {1}"))
                {
                    statusMessages.Add($"Item number {i} has invalid values ({ string.Join("; ", validationResults) })");
                    continue;
                }

                try
                {
                    var createdCategory = CreateProductCategory(productCategory.ProductCategory, siteId);
                    createdCategory.ProductCategoryTitle = createdCategory.DocumentName;
                    createdCategory.ProductCategoryDescription = productCategory.Description;
                    ClearImage(createdCategory);
                    if (!string.IsNullOrWhiteSpace(productCategory.ImageURL))
                    {
                        var imageUrl = LoadImageToLibrary(createdCategory, productCategory.ImageURL);
                        SetImage(createdCategory, imageUrl);
                    }
                    createdCategory.Update();
                }
                catch (Exception ex)
                {
                    statusMessages.Add($"There was an error when processing item #{i} : {ex.Message}");
                    EventLogProvider.LogException("Import product categories", "EXCEPTION", ex);
                }
            }

            CacheHelper.ClearCache();
            return new ImportResult
            {
                AllMessagesCount = statusMessages.AllMessagesCount,
                ErrorMessages = statusMessages.ToArray()
            };
        }

        private string LoadImageToLibrary(TreeNode createdCategory, string imageURL)
        {
            var library = new MediaLibrary
            {
                SiteId = createdCategory.NodeSiteID,
                LibraryName = "ProductCategoriesImages",
                LibraryFolder = "ProductCategories",
                LibraryDescription = "Media library for storing product categories images."
            };
            return library.DownloadImageToMedialibrary(imageURL
                , $"Image{createdCategory.NodeAlias}"
                , $"Product image for category {createdCategory.GetValue("ProductCategoryTitle")}");
        }

        private void ClearImage(ProductCategory category)
        {
            MediaLibrary.RemoveMediaFile(GetImage(category));
            SetImage(category, string.Empty);
        }

        private void SetImage(ProductCategory category, string image)
        {
            category.ProductCategoryImage = image;
        }

        private string GetImage(ProductCategory category)
        {
            return category?.ProductCategoryImage;
        }
    }
}