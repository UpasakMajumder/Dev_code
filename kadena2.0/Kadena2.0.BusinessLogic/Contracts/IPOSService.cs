namespace Kadena.BusinessLogic.Contracts
{
    public interface IPOSService
    {
        void TogglePOSStatus(int posID);
        bool DeletePOS(int posID);
    }
}
