namespace Kadena.BusinessLogic.Factories
{
    public interface  IOrderResultPageUrlFactory
    {
        string GetOrderResultPageUrl(bool orderSuccess, string orderId);
        string GetCardPaymentResultPageUrl(bool orderSuccess, string orderId, string submissionId = "", string error = "");
    }
}
