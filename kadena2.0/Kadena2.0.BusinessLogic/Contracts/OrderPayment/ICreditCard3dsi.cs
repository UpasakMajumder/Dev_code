using Kadena.Dto.SubmitOrder.MicroserviceRequests;
using Kadena.Models.CreditCard;
using Kadena.Models.SubmitOrder;
using System.Threading.Tasks;

namespace Kadena2.BusinessLogic.Contracts.OrderPayment
{
    public interface ICreditCard3dsi
    {
        Task<SubmitOrderResult> PayByCard3dsi(OrderDTO orderData);
        Task<bool> SaveToken(SaveTokenData tokenData);
        bool CreditcardSaved(string submissionId);
    }
}
