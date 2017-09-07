using System.Collections.Generic;

namespace Kadena.Dto.Product.Responses
{
    public class GetProductsDto
    {
        public List<ProductCategoryDto> Categories { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
