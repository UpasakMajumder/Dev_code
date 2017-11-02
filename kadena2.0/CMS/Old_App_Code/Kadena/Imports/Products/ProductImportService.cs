using CMS.DocumentEngine;
using CMS.EventLog;
using CMS.Localization;
using CMS.Membership;
using Kadena.Models.Product;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.Old_App_Code.Kadena.Imports.Products
{
    public class ProductImportService : ImportServiceBase
    {
        public ImportResult ProcessImportFile(byte[] importFileData, ExcelType type, int siteID)
        {
            var site = GetSite(siteID);
            var rows = GetExcelRows(importFileData, type);
            var products = GetDtosFromExcelRows<ProductDto>(rows);
            var statusMessages = new List<string>();

            var currentItemNumber = 1;
            foreach (var productDto in products)
            {
                List<string> validationResults;
                if (!ValidateImportItem(productDto, out validationResults))
                {
                    statusMessages.Add($"Item number {currentItemNumber} has invalid values ({ string.Join("; ", validationResults) })");
                    continue;
                }

                try
                {
                    SaveProduct(productDto);
                }
                catch (Exception ex)
                {
                    statusMessages.Add("There was an error when processing item number " + currentItemNumber);
                    EventLogProvider.LogException("Import users", "EXCEPTION", ex);
                }

                currentItemNumber++;
            }

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

        private void SaveProduct(ProductDto productDto)
        {
            var categories = productDto.ProductCategory.Split('\n');
            var productParent = CreateProductCategory(categories);
            var newProduct = AppendProduct(productParent, productDto);
        }

        private TreeNode AppendProduct(TreeNode parent, ProductDto product)
        {
            if (parent == null)
                return null;

            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            TreeNode newPage = TreeNode.New("KDA.Product", tree);
            newPage.DocumentName = product.ProductName;
            newPage.DocumentCulture = "en-us";
            newPage.SetValue("ProductType", product.ProductType);
            newPage.SetValue("ProductSKUWeight", Convert.ToDecimal(product.PackageWeight));
            newPage.SetValue("ProductNumberOfItemsInPackage", Convert.ToInt32(product.ItemsInPackage));

            newPage.SetValue("ProductChiliTemplateID", product.ChiliTemplateID ?? string.Empty);
            newPage.SetValue("ProductChiliWorkgroupID", product.ChiliWorkgroupID?? string.Empty);
            newPage.SetValue("ProductChiliPdfGeneratorSettingsId", product.ChiliPdfGeneratorSettingsID ?? string.Empty);

            // Inserts the new page as a child of the parent page
            newPage.Insert(parent);
            return newPage;
        }

        private TreeNode CreateProductCategory(string[] path)
        {
            var root = DocumentHelper.GetDocuments("KDA.ProductsModule")
                            .Path("/", PathTypeEnum.Children)
                            .WhereEquals("ClassName", "KDA.ProductsModule")
                            .Culture(LocalizationContext.CurrentCulture.CultureCode)
                            .CheckPermissions()
                            .NestingLevel(1)
                            .OnCurrentSite()
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

                // Inserts the new page as a child of the parent page
                category.Insert(parentPage);
            }

            return AppendProductCategory(category, subnodes.Skip(1).ToArray());
        }
    }
}