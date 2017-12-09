using Kadena.BusinessLogic.Contracts;
using Kadena.WebAPI.KenticoProviders.Contracts;

namespace Kadena.BusinessLogic.Services
{
    public class AddressBookService : IAddressBookService
    {
        private readonly IKenticoAddressBookProvider kenticoAddress;

        public AddressBookService(IKenticoAddressBookProvider kenticoAddress)
        {
            this.kenticoAddress = kenticoAddress;
        }

        public void DeleteAddress(int addressID)
        {
            kenticoAddress.DeleteAddress(addressID);
        }
    }
}
