using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Services
{
    public class UserBudgetService : IUserBudgetService
    {
        private readonly IkenticoUserBudgetProvider kenticoUserBudget;

        public UserBudgetService(IkenticoUserBudgetProvider kenticoUserBudget)
        {
            if (kenticoUserBudget == null)
            {
                throw new ArgumentNullException(nameof(kenticoUserBudget));
            }
            this.kenticoUserBudget = kenticoUserBudget;
        }

        public bool UpdateUserBudgetAllocation(int itemID, double userBudget)
        {
            return kenticoUserBudget.UpdateUserBudgetAllocation(itemID, userBudget);
        }
    }
}
