using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.BusinessLogic.Services
{
    public class POSService : IPOSService
    {
        private readonly IKenticoPOSProvider kenticoPOS;

        public POSService(IKenticoPOSProvider kenticoPOS)
        {
            this.kenticoPOS = kenticoPOS;
        }

        public void TogglePOSStatus(int posID)
        {
            kenticoPOS.TogglePOSStatus(posID);
        }
    }
}
