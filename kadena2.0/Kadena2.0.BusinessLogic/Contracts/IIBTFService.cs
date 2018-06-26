using Kadena.Dto.SubmitOrder.MicroserviceRequests;

namespace Kadena.BusinessLogic.Contracts
{
    public interface IIBTFService
    {
        void InsertIBTFAdjustmentRecord(OrderDTO order);

        void UpdateRemainingBudget(int campaignID);
    }
}