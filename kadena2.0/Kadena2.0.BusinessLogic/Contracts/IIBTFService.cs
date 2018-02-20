using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Models.Brand;
using Kadena.Models.IBTF;
using System.Collections.Generic;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IIBTFService
    {
        void InsertIBTFAdjustmentRecord(OrderDTO order);

        void UpdateRemainingBudget(int campaignID);
    }
}