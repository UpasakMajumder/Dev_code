using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.BusinessLogic.Services
{
    public class CampaignsService : ICampaignsService
    {
        private readonly IKenticoCampaignsProvider kenticoCampaigns;

        public CampaignsService(IKenticoCampaignsProvider kenticoCampaigns)
        {
            if (kenticoCampaigns == null)
            {
                throw new ArgumentNullException(nameof(kenticoCampaigns));
            }
            this.kenticoCampaigns = kenticoCampaigns;
        }

        public void DeleteCampaign(int campaignID)
        {
            kenticoCampaigns.DeleteCampaign(campaignID);
        }
    }
}
