using Kadena.Models.RecentOrders;
using CMS.DocumentEngine;
using Kadena.Models.CampaignData;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoCampaignsProvider
    {
        void DeleteCampaign(int campaignID);

        OrderCampaginHead GetCampaigns(string orderType);

        bool CloseCampaignIBTF(int campaignID);

        int GetOpenCampaignID();

        string GetCampaignFiscalYear(int campaignID);

        CampaignData GetCampaign(int campaignID);
    }
}