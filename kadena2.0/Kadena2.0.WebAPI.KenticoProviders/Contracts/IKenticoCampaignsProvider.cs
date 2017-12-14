using Kadena.Models.Campaigns;
using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoCampaignsProvider
    {
        void DeleteCampaign(int campaignID);
        List<Campaign> GetCampaigns(string orderType);
    }
}
