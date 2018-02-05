using Kadena.Models.CreditCard;
using Kadena.Models.SubmitOrder;
using System.Threading.Tasks;

namespace Kadena2.BusinessLogic.Contracts.OrderPayment
{
    public interface ICreditCard3dsi
    {
        Task<SubmitOrderResult> PayByCard3dsi(SubmitOrderRequest orderRequest);
        Task<bool> SaveToken(SaveTokenData tokenData);
        string CreditcardSaved(string submissionId);
    }
}
