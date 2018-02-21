using Kadena.Models.SubmitOrder;
using System.Threading.Tasks;

namespace Kadena2.BusinessLogic.Contracts.OrderPayment
{
    public interface ISavedCreditCard3dsi
    {
        Task<SubmitOrderResult> PayBySavedCard3dsi(SubmitOrderRequest orderRequest);
    }
}
