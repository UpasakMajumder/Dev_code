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

        public void UpdateUserBudgetAllocationRecords(int userId, string year, decimal? totalToBeDeducted)
        {
            var userBudgetDetails = CustomTableItemProvider.GetItems(CustomTableClassName).WhereEquals("UserID", userId).WhereEquals("Year", year).FirstOrDefault();
            userBudgetDetails.SetValue("UserRemainingBudget", userBudgetDetails.GetValue("Budget", default(decimal)) - totalToBeDeducted);
            userBudgetDetails.Update();
        }

        public List<CustomTableItem> GetFiscalYearRecords()
        {
            var fiscalYearData = CustomTableItemProvider.GetItems(FiscalYearClassName).ToList();
            return fiscalYearData;
        }
    }
}
