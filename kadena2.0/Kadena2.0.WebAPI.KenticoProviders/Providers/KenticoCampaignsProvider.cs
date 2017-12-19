using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.SiteProvider;
using Kadena.Models.RecentOrders;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders
{
    public class KenticoCampaignsProvider : IKenticoCampaignsProvider
    {
        private readonly string PageTypeClassName = "KDA.Campaign";
        private readonly string orderTypePreBuy = "prebuy";

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

        public OrderCampaginHead GetCampaigns(string orderType)
        {
            List<OrderCampaginItem> campaigns = new List<OrderCampaginItem>();
            if (orderType.Equals(orderTypePreBuy))
            {
                var campaignDocuments = DocumentHelper.GetDocuments(PageTypeClassName).OnSite(SiteContext.CurrentSiteID).WhereTrue("OpenCampaign");
                if (!DataHelper.DataSourceIsEmpty(campaignDocuments))
                {
                    foreach (TreeNode item in campaignDocuments.TypedResult.Items)
                    {
                        campaigns.Add(new OrderCampaginItem()
                        {
                            id = item.GetIntegerValue("CampaignID", 0),
                            name = item.GetStringValue("Name", string.Empty)
                        });
                    }
                }
            }
            return new OrderCampaginHead()
            {
                placeholder = ResHelper.GetString("Kadena.RecentOrders.Filter.SelectCampaign"),
                items = campaigns
            };
        }
    }
}
