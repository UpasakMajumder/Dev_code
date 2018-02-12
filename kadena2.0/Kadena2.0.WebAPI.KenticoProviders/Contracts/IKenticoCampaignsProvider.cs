using Kadena.Models.RecentOrders;
using Kadena.Models.CampaignData;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoCampaignsProvider
    {
        void DeleteCampaign(int campaignID);
        OrderCampaginHead GetCampaigns(string orderType);
        bool CloseCampaignIBTF(int campaignID);
        string GetCampaignFiscalYear(int campaignID);
    }
}
