using CMS.EventLog;
using Kadena.Models.Product;
using System;
using System.Collections.Generic;

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
                try
                {
                    List<string> validationResults;
                    if (!ValidateImportItem(productDto, out validationResults))
                    {
                        // sort errors by field
                        validationResults.Sort();

                        statusMessages.Add($"Item number {currentItemNumber} has invalid values ({ string.Join("; ", validationResults) })");
                        continue;
                    }

                    /*var newUser = CreateUser(userDto, site);
                    var newCustomer = CreateCustomer(newUser.UserID, siteID, userDto);
                    CreateCustomerAddress(newCustomer.CustomerID, userDto);
                    emailService.SendResetPasswordEmail(newUser, passwordEmailTemplateName, site.SiteName);*/
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
    }
}