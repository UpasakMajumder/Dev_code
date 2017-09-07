using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;
using Kadena.Models.Product;
using CMS.DocumentEngine;
using CMS.Membership;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoProductsProvider : IKenticoProductsProvider
    {
        public List<ProductCategoryLink> GetCategories(string path)
        {
            var tree = new TreeProvider(MembershipContext.AuthenticatedUser);

            var pages = tree.SelectNodes()
                            .Path(path, PathTypeEnum.Children);
                            //.WhereEquals("DocumentName", "Coffee%")
                            //.OnSite("DancingGoat")
                            //.Culture("en-us");


            foreach (TreeNode page in pages)
            {
                //page.DocumentName = "Updated article name";
                //page.SetValue("ArticleTitle", "Updated article title");

                // Updates the page in the database
                //page.Update();
            }

            return null;
        }

        public List<ProductLink> GetProducts(string path)
        {
            return null;
        }
    }
}
