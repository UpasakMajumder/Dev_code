using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.BusinessLogic.Services
{
    public class POSService : IPOSService
    {
        private readonly IKenticoPOSProvider kenticoPOS;

        public POSService(IKenticoPOSProvider kenticoPOS)
        {
            if (kenticoPOS == null)
            {
                throw new ArgumentNullException(nameof(kenticoPOS));
            }
            this.kenticoPOS = kenticoPOS;
        }

        public void TogglePOSStatus(int posID)
        {
            kenticoPOS.TogglePOSStatus(posID);
        }
        public bool DeletePOS(int posID)
        {
           return kenticoPOS.DeletePOS(posID);
        }
    }
}
