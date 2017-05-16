using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.KenticoApi.Objects
{
    public class ProductModul : CmsDocument
    {
        public string ModuleMainIcon { get; set; }
        public string ModuleName { get; set; }
        public int ProductsModuleID { get; set; }
    }
}
