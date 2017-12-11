using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.BusinessLogic.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IKenticoProductCategoryProvider kenticoProductCategory;

        public ProductCategoryService(IKenticoProductCategoryProvider kenticoProductCategory)
        {
            if (kenticoProductCategory == null)
            {
                throw new ArgumentNullException(nameof(kenticoProductCategory));
            }
            this.kenticoProductCategory = kenticoProductCategory;
        }

        public void DeleteCategory(int categoryID)
        {
            kenticoProductCategory.DeleteCategory(categoryID);
        }
    }
}
