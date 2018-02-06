using CMS.CustomTables;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Linq;

namespace Kadena.WebAPI.KenticoProviders.Providers
{
    public class KenticoUserBudgetProvider : IkenticoUserBudgetProvider
    {
        public bool UpdateUserBudgetAllocation(int itemID, double userBudget)
        {
            var userBudgetDetails = CustomTableItemProvider.GetItems("KDA.UserFYBudgetAllocation").WhereEquals("ItemID", itemID).FirstOrDefault();
            if (userBudgetDetails != null)
            {
                userBudgetDetails.SetValue("Budget", userBudget);
                userBudgetDetails.Update();
                return true;
            }
            return false;
        }
    }
}
