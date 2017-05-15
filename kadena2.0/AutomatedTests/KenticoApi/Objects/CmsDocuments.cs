using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedTests.KenticoApi.Objects
{ 
    public class CmsDocuments
    {
        public List<ProductModul> KDA_ProductsModule { get; set; }
        public List<ProductCategory> KDA_ProductCategory { get; set; }
        public List<Product> KDA_Product { get; set; }
    }
}
