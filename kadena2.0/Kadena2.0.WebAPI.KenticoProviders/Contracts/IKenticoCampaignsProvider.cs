using Kadena.Models.RecentOrders;
using CMS.DocumentEngine;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoCampaignsProvider
    {
        void DeleteCampaign(int campaignID);
        OrderCampaginHead GetCampaigns(string orderType);
        bool CloseCampaignIBTF(int campaignID);
        string GetCampaignFiscalYear(int campaignID);
        TreeNode GetCampaign(int campaignID);
    }
}
