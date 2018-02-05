namespace Kadena.BusinessLogic.Factories
{
    public interface  IOrderResultPageUrlFactory
    {
        string GetOrderResultPageUrl(string baseUrl, bool orderSuccess, string orderId);
    }
}
