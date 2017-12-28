using Kadena.Models.RecentOrders;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ICampaignsService
    {
        void DeleteCampaign(int campaignID);
        OrderCampaginHead GetCampaigns(string orderType);
    }
}
