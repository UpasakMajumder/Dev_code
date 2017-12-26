namespace Kadena2.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoOrderProvider
    {
        int GetOrderStatusId(string name);
        string MapOrderStatus(string microserviceStatus);
    }
}
