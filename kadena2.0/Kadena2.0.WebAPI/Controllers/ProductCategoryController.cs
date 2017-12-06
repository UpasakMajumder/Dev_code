using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Membership;
using CMS.Search;
using Kadena.WebAPI.Infrastructure;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    public class ProductCategoryController : ApiControllerBase
    {
        [HttpDelete]
        [Route("api/productcategory/{CategoryID}")]
        public bool DeleteCategory(int CategoryID)
        {
            bool status = false;
            if (CategoryID > 0)
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                TreeNode page = tree.SelectNodes("KDA.ProductCategory").Where("ProductCategoryID", QueryOperator.Equals, CategoryID).OnCurrentSite();
                if (page != null)
                {
                    status = page.Delete();
                    if (SearchIndexInfoProvider.SearchEnabled)
                    {
                        SearchTaskInfoProvider.CreateTask(SearchTaskTypeEnum.Delete, TreeNode.OBJECT_TYPE, SearchFieldsConstants.ID, page.GetSearchID(), page.DocumentID);
                    }
                }
            }
            return status;
        }
    }
}
