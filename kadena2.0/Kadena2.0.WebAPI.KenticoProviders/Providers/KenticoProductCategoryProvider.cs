using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Membership;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoProductCategoryProvider : IKenticoProductCategoryProvider
    {
        private readonly string PageTypeClassName = "KDA.POSNumber";

        public void DeleteCategory(int categoryID)
        {
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            TreeNode productCategory = tree.SelectNodes(PageTypeClassName)
                                            .Where("ProductCategoryID", QueryOperator.Equals, categoryID)
                                            .OnCurrentSite();
            if (productCategory != null)
            {
                productCategory.Delete();
            }
        }
    }
}
