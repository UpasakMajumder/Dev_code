using CMS.SiteProvider;
using System;
using System.Collections.Generic;

namespace Kadena.Old_App_Code.Kadena.Imports
{
    public abstract class ImportServiceBase
    {
        protected List<string[]> GetExcelRows(byte[] fileData, ExcelType type)
        {
            var rows = new ExcelReader().ReadDataFromExcelFile(fileData, type);
            if (rows.Count <= 1)
            {
                throw new Exception("The file contains no data");
            }
            return rows;
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
    }
}