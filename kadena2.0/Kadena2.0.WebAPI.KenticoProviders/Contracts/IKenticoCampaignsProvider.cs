using CMS.DocumentEngine;
using Kadena.Models.RecentOrders;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoCampaignsProvider
    {
        void DeleteCampaign(int campaignID);
        TreeNode GetCampaign(int campaignID);
        OrderCampaginHead GetCampaigns(string orderType);
        bool CloseCampaignIBTF(int campaignID);
    }
}
