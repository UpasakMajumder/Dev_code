using CMS.CustomTables;
using CMS.Helpers;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders.Providers
{
    public class KenticoUserBudgetProvider : IkenticoUserBudgetProvider
    {
        private readonly string CustomTableClassName = "KDA.UserFYBudgetAllocation";

        private readonly string FiscalYearClassName = "KDA.FiscalYearManagement";
        public bool UpdateUserBudgetAllocation(int itemID, double userBudget)
        {
            var userBudgetDetails = CustomTableItemProvider.GetItems(CustomTableClassName).WhereEquals("ItemID", itemID).FirstOrDefault();
            if (userBudgetDetails != null)
            {
                userBudgetDetails.SetValue("Budget", userBudget);
                userBudgetDetails.Update();
                return true;
            }
            return false;
        }

        public List<CustomTableItem> GetUserBudgetAllocationRecords(int userID, int siteID)
        {
            var userBudgetData = CustomTableItemProvider.GetItems(CustomTableClassName).WhereEquals("UserID", userID).WhereEquals("SiteID", siteID).ToList();
            return userBudgetData;
        }

        public List<CustomTableItem> GetFiscalYearRecords()
        {
            var fiscalYearData = CustomTableItemProvider.GetItems(FiscalYearClassName).ToList();
            return fiscalYearData;
        }

    }
}
