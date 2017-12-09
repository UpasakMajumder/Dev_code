using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Membership;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoCampaignsProvider : IKenticoCampaignsProvider
    {
        private readonly string PageTypeClassName = "KDA.Campaign";

        public void DeleteCampaign(int campaignID)
        {
            TreeProvider tree = new TreeProvider(MembershipContext.AuthenticatedUser);
            TreeNode campaign = tree.SelectNodes(PageTypeClassName)
                                    .Where("CampaignID", QueryOperator.Equals, campaignID)
                                    .OnCurrentSite();
            if (campaign != null)
            {
                campaign.Delete();
            }
        }
    }
}
