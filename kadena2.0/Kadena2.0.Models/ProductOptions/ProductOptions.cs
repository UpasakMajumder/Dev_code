using System.Collections.Generic;

namespace Kadena.Models.ProductOptions
{
    public class ProductOptions
    {
        public string PriceElementId { get; set; }
        public string PriceUrl { get; set; }
        public IList<ProductCategory> Categories { get; set; }
    }
}
