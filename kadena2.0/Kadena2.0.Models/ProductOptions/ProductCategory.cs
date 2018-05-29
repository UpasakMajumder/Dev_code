using System.Collections.Generic;

namespace Kadena.Models.ProductOptions
{
    public class ProductCategory
    {
        public string Name { get; set; }
        public string Selector { get; set; }
        public IList<ProductOption> Options { get; set; }
    }
}
