using CMS.CustomTables;
using Kadena.Models;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IkenticoUserBudgetProvider
    {
        string UpdateUserBudgetAllocation(int itemID, double userBudget);

        List<CustomTableItem> GetUserBudgetAllocationRecords(int userID, int siteId);
        bool CheckIfYearExists(string year, int userId);

        void UpdateUserBudgetAllocationRecords(int userId, string year, decimal? totalToBeDeducted);

        List<CustomTableItem> GetFiscalYearRecords();

        CustomTableItem CreateUserBudgetWithYear(string year, int siteID, int userId);

    }
}
