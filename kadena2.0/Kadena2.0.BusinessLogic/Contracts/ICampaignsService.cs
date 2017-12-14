using Kadena.Models.Campaigns;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ICampaignsService
    {
        void DeleteCampaign(int campaignID);
        List<Campaign> GetCampaigns(string orderType);
    }
}
