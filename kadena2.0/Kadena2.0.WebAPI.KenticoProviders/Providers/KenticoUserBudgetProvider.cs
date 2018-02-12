using CMS.CustomTables;
using CMS.DataEngine;
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
        public string UpdateUserBudgetAllocation(int itemID, double userBudget)
        {
            var userBudgetDetails = CustomTableItemProvider.GetItems(CustomTableClassName).WhereEquals("ItemID", itemID).FirstOrDefault();
            if (userBudgetDetails != null)
            {
                userBudgetDetails.SetValue("Budget", userBudget);
                userBudgetDetails.Update();
                userBudgetDetails.SetValue("UserRemainingBudget", userBudgetDetails.GetValue("UserRemainingBudget", default(decimal)) + (userBudgetDetails.GetValue("Budget", default(decimal)) - userBudgetDetails.GetValue("UserRemainingBudget", default(decimal))));
                userBudgetDetails.Update();
                return userBudgetDetails.GetValue("UserRemainingBudget", string.Empty);
            }
            
            return string.Empty;
        }

        public List<CustomTableItem> GetUserBudgetAllocationRecords(int userId, int siteId)
        {
            var userBudgetDetails = CustomTableItemProvider.GetItems(CustomTableClassName).WhereEquals("UserID", userId).WhereEquals("SiteID", siteId).ToList();
            return userBudgetDetails;
        }

        public bool CheckIfYearExists(string year, int userId)
        {
            var userBudgetDetails = CustomTableItemProvider.GetItems(CustomTableClassName).WhereEquals("UserID", userId).WhereEquals("Year", year).ToList();
            return !DataHelper.DataSourceIsEmpty(userBudgetDetails) ? true : false;

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

        public CustomTableItem CreateUserBudgetWithYear(string year, int siteID, int userId)
        {
            DataClassInfo customTable = DataClassInfoProvider.GetDataClassInfo(CustomTableClassName);
            CustomTableItem newCustomTableItem = CustomTableItem.New(CustomTableClassName);
            if (customTable != null)
            {
                newCustomTableItem.SetValue("UserID", userId);
                newCustomTableItem.SetValue("Year", year);
                newCustomTableItem.SetValue("Budget", default(decimal));
                newCustomTableItem.SetValue("UserRemainingBudget", default(decimal));
                newCustomTableItem.SetValue("SiteID", siteID);
                newCustomTableItem.Insert();
                return newCustomTableItem;
            }
            return newCustomTableItem;
        }
    }
}
