using CMS.CustomTables;
using Kadena.Models;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IkenticoUserBudgetProvider
    {
        bool UpdateUserBudgetAllocation(int itemID, double userBudget);

        List<CustomTableItem> GetUserBudgetAllocationRecords(int userID, int siteID);

        List<CustomTableItem> GetFiscalYearRecords();

    }
}
