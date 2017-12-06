using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Membership;
using CMS.Search;
using Kadena.WebAPI.Infrastructure;
using System.Web.Http;

namespace Kadena.WebAPI.Controllers
{
    public class CampaignsController : ApiControllerBase
    {
        [HttpDelete]
        [Route("api/campaigns/{CampaignID}")]
        public bool DeleteCampaign(int CampaignID)
        {
            bool status = false;
            if (CampaignID > 0)
            {
                TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
                TreeNode page = tree.SelectNodes("KDA.Campaign").Where("CampaignID", QueryOperator.Equals, CampaignID).OnCurrentSite();
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
