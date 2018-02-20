using System;
using System.Collections.Generic;

namespace Kadena.Models.Product
{
    public class ProductsPage
    {
        public List<ProductCategoryLink> Categories { get; set; }
        public List<ProductLink> Products { get; set; }
        public void MarkFavoriteProducts(List<int> favoriteIds)
        {
            Products.ForEach(p => p.IsFavourite = (favoriteIds?.Contains(p.Id) ?? false));
        }
    }
}
