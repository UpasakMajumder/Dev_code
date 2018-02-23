using Kadena.Models.SubmitOrder;

namespace Kadena.BusinessLogic.Factories
{
    public interface  IOrderResultPageUrlFactory
    {
        string GetOrderResultPageUrl(SubmitOrderResult orderResult);
        string GetCardPaymentResultPageUrl(bool orderSuccess, string orderId, string submissionId = "", string error = "");
    }
}
