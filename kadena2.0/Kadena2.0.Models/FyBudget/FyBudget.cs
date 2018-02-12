using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.Models.FyBudget
{
    public class FyBudget
    {
        public int ItemID { get; set; }
        public int UserID { get; set; }
        public decimal UserRemainingBudget { get; set; }
        public decimal Budget { get; set; }
        public bool IsYearEnded { get; set; }
        public string Year { get; set; }
    }

    public class FiscalYear
    {
        public int ItemID { get; set; }
        public string Year { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
