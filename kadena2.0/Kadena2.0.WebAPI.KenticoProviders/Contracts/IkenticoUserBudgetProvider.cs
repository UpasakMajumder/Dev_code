using Kadena.Models.FyBudget;
using Kadena.Models.UserBudget;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoUserBudgetProvider
    {
        string UpdateUserBudgetAllocation(int itemID, decimal userBudget);

        List<UserBudgetItem> GetUserBudgetAllocationRecords(int userID, int siteId);

        List<FiscalYear> GetFiscalYearRecords();

        UserBudgetItem GetOrCreateUserBudgetWithYear(string year, int siteID, int userId);

        void AdjustUserRemainingBudget(string year, int userID, decimal adjustment);
    }
}
