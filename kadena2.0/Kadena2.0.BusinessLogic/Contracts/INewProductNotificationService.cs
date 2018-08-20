namespace Kadena.BusinessLogic.Contracts
{
    public interface INewProductNotificationService
    {
        void SendNotification(int newSKUID);
    }
}
