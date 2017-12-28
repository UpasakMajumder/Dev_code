using CMS.SiteProvider;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.Old_App_Code.Kadena.Imports
{
    public abstract class ImportServiceBase
    {
        protected StatusMessages statusMessages = new StatusMessages(10);

        protected List<string[]> GetExcelRows(byte[] fileData, ExcelType type)
        {
            var rows = ExcelReader.ReadDataFromExcelFile(fileData, type);
            if (rows.Count <= 1)
            {
                throw new Exception("The file contains no data");
            }
            return rows;
        }

        protected List<T> GetDtosFromExcelRows<T>(List<string[]> rows) where T:class
        {
            var header = rows.First();
            var mapper = ImportHelper.GetImportMapper<T>(header);
            var values = rows.Skip(1)
                .Select(row => mapper(row))
                .ToList();
            return values;
        }

        protected SiteInfo GetSite(int siteID)
        {
            var site = SiteInfoProvider.GetSiteInfo(siteID);
            if (site == null)
            {
                throw new Exception("Invalid site id " + siteID);
            }
            return site;
        }

        public abstract ImportResult Process(byte[] importFileData, ExcelType type, int siteId);
    }
}