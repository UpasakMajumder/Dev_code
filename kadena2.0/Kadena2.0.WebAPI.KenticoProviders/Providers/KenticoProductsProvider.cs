using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;
using Kadena.Models.Product;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoProductsProvider : IKenticoProductsProvider
    {
        public List<ProductCategoryLink> GetCategories(string path)
        {
            return null;
        }

        public List<ProductLink> GetProducts(string path)
        {
            return null;
        }
    }
}
