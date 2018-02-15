using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kadena.ScheduledTasks.Infrastructure
{
    interface IOrderCreationService
    {
        string GenerateOrder(int openCampaignID, int campaignClosingUserID);
    }
}
