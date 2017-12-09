using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.BusinessLogic.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IKenticoProductCategoryProvider kenticoProductCategory;

        public ProductCategoryService(IKenticoProductCategoryProvider kenticoProductCategory)
        {
            this.kenticoProductCategory = kenticoProductCategory;
        }

        public void DeleteCategory(int categoryID)
        {
            kenticoProductCategory.DeleteCategory(categoryID);
        }
    }
}
