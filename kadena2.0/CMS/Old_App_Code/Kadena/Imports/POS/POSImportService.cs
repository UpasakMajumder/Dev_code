using CMS.EventLog;
using CMS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using CMS.CustomTables.Types.KDA;
using CMS.CustomTables;

namespace Kadena.Old_App_Code.Kadena.Imports.POS
{
    public class POSImportService : ImportServiceBase
    {
        private static List<POSNumberItem> _currentPOSList;
        private static List<BrandItem> _brands;
        private static List<POSCategoryItem> _posCategories;

        public override ImportResult Process(byte[] importFileData, ExcelType type, int siteID)
        {
            CacheHelper.ClearCache();
            statusMessages.Clear();

            _brands = CustomTableItemProvider.GetItems<BrandItem>().ToList();
            _posCategories = CustomTableItemProvider.GetItems<POSCategoryItem>().ToList();
            _currentPOSList = CustomTableItemProvider.GetItems<POSNumberItem>().ToList();
            if (_brands.Count <= 0 || _posCategories.Count <= 0)
            {
                statusMessages.Add(ResHelper.GetString("Kadena.POS.BulkImport.NoBrandsAndPOSCategories"));
            }
            else
            {
                var rows = GetExcelRows(importFileData, type);
                var poslist = GetDtosFromExcelRows<POSDto>(rows);

                var currentItemNumber = 0;
                foreach (var pos in poslist)
                {
                    currentItemNumber++;

                    List<string> validationResults;
                    if (!ValidateImportItem(pos, out validationResults))
                    {
                        statusMessages.Add($"Item number {currentItemNumber} has invalid values ({ string.Join("; ", validationResults) })");
                        continue;
                    }

                    try
                    {
                        SavePOS(pos);
                    }
                    catch (Exception ex)
                    {
                        statusMessages.Add($"There was an error when processing item #{currentItemNumber} : {ex.Message}");
                        EventLogProvider.LogException("Import POS", "EXCEPTION", ex);
                    }
                }
            }

            CacheHelper.ClearCache();

            return new ImportResult
            {
                AllMessagesCount = statusMessages.AllMessagesCount,
                ErrorMessages = statusMessages.ToArray()
            };
        }

        private static bool ValidateImportItem(POSDto pos, out List<string> validationErrors)
        {
            var errorMessageFormat = "field {0} - {1}";
            bool isValid = ValidatorHelper.ValidateDto(pos, out validationErrors, errorMessageFormat);

            if (!isValid)
            {
                return false;
            }
            if (IsPOSNumberExists(GetPOSNumber(pos)))
            {
                isValid = false;
                validationErrors.Add(ResHelper.GetString("Kadena.POS.BulkImport.POSAlreadyExists"));
            }

            return isValid;
        }

        private static bool IsPOSNumberExists(string posNumber)
        {
            POSNumberItem pos = _currentPOSList.Where(x => x.POSNumber.ToString().Equals(posNumber)).FirstOrDefault();
            return pos != null;
        }

        private static string GetPOSNumber(POSDto posDto)
        {
            int year = ValidationHelper.GetInteger(posDto.Year, default(int));
            if (year != default(int))
            {
                return $"{GetBrandCode(posDto.Brand)}{(year % 100)}{posDto.POSCode}";
            }
            else
            {
                return string.Empty;
            }
        }

        private static string GetBrandCode(string brandName)
        {
            string brandCode = string.Empty;
            BrandItem brand = GetBrand(brandName);
            if (brand != null)
            {
                brandCode = brand.BrandCode.ToString();
            }
            return brandCode;
        }

        private static BrandItem GetBrand(string brandName)
        {
            return _brands.Where(x => x.BrandName.Equals(brandName)).FirstOrDefault();
        }

        private static POSCategoryItem GetPOSCategory(string posCategoryName)
        {
            return _posCategories.Where(x => x.PosCategoryName.Equals(posCategoryName)).FirstOrDefault();
        }

        private static void SavePOS(POSDto posDto)
        {
            POSCategoryItem posCategory = GetPOSCategory(posDto.POSCategory);
            BrandItem brand = GetBrand(posDto.Brand);
            if (posCategory != null && brand != null)
            {
                POSNumberItem pos = new POSNumberItem()
                {
                    BrandID = brand.BrandCode,
                    BrandName = brand.BrandName,
                    Year = ValidationHelper.GetInteger(posDto.Year, default(int)),
                    POSCode = ValidationHelper.GetInteger(posDto.POSCode, default(int)),
                    POSNumber = ValidationHelper.GetInteger(GetPOSNumber(posDto), default(int)),
                    POSCategoryID = posCategory.ItemID,
                    POSCategoryName = posCategory.PosCategoryName
                };
                pos.Insert();
                _currentPOSList.Add(pos);
            }
        }
    }
}