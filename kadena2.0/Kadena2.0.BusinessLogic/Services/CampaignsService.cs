using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.BusinessLogic.Services
{
    public class CampaignsService : ICampaignsService
    {
        private readonly IKenticoCampaignsProvider kenticoCampaigns;

        public CampaignsService(IKenticoCampaignsProvider kenticoCampaigns)
        {
            this.kenticoCampaigns = kenticoCampaigns;
        }

        public void DeleteCampaign(int campaignID)
        {
            kenticoCampaigns.DeleteCampaign(campaignID);
        }
    }
}
