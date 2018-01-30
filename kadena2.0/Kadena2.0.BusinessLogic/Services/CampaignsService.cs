using Kadena.BusinessLogic.Contracts;
using Kadena.Models.RecentOrders;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;
using System.Collections.Generic;

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

        public OrderCampaginHead GetCampaigns(string orderType)
        {
            return kenticoCampaigns.GetCampaigns(orderType);
        }

        public bool CloseCampaignIBTF(int campaignID)
        {
            return kenticoCampaigns.CloseCampaignIBTF(campaignID);
        }
    }
}
