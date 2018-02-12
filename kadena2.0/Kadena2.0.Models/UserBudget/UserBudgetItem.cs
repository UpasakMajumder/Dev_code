using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.Models.UserBudget
{
    public class UserBudgetItem
    {
        public int ItemID { get; set; }
        public decimal Budget { get; set; }
        public int UserID { get; set; }
        public decimal UserRemainingBudget { get; set; }
    }
}
