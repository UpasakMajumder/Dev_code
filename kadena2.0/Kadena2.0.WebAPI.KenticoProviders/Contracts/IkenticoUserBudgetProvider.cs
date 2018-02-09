using CMS.CustomTables;
using Kadena.Models;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IkenticoUserBudgetProvider
    {
        bool UpdateUserBudgetAllocation(int itemID, double userBudget);

        void UpdateUserBudgetAllocationRecords(int userId, string year, decimal? totalToBeDeducted);

        List<CustomTableItem> GetFiscalYearRecords();

    }
}
