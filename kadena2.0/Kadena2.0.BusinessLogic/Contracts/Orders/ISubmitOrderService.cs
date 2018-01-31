using Kadena.Models.SubmitOrder;
using System.Threading.Tasks;

namespace Kadena.BusinessLogic.Contracts
{
    public interface ISubmitOrderService
    {
        Task<SubmitOrderResult> SubmitOrder(SubmitOrderRequest request);
    }
}
