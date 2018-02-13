using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IUserBudgetService
    {
        string UpdateUserBudgetAllocation(int itemID, double userBudget);
    }
}
