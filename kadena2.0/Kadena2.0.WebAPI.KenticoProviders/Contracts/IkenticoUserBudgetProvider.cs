using CMS.CustomTables;
using Kadena.Models;
using Kadena.Models.FyBudget;
using Kadena.Models.UserBudget;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IkenticoUserBudgetProvider
    {
        string UpdateUserBudgetAllocation(int itemID, double userBudget);

        List<UserBudgetItem> GetUserBudgetAllocationRecords(int userID, int siteId);
        bool CheckIfYearExists(string year, int userId);

        void UpdateUserBudgetAllocationRecords(int userId, string year, decimal? totalToBeDeducted);

        List<FiscalYear> GetFiscalYearRecords();

        UserBudgetItem CreateUserBudgetWithYear(string year, int siteID, int userId);

    }
}
