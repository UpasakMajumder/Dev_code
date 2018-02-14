using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IFailedOrderStatusProvider
    {
        void InsertCampaignOrdersInProgress(int campaignID);
        void UpdateCampaignOrderStatus(int campaignID);
        bool GetCampaignOrderStatus(int campaignID);
        void UpdatetFailedOrders(int campaignID, bool IsFailed);
    }
}
