using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;
using System;

namespace Kadena.BusinessLogic.Services
{
    public class AddressBookService : IAddressBookService
    {
        private readonly IKenticoAddressBookProvider kenticoAddress;

        public AddressBookService(IKenticoAddressBookProvider kenticoAddress)
        {
            if (kenticoAddress == null)
            {
                throw new ArgumentNullException(nameof(kenticoAddress));
            }
            this.kenticoAddress = kenticoAddress;
        }

        public void DeleteAddress(int addressID)
        {
            kenticoAddress.DeleteAddress(addressID);
        }
    }
}
