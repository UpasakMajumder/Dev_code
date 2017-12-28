using System.Collections.Generic;

namespace Kadena.WebAPI.KenticoProviders.Contracts
{
    public interface IKenticoAddressBookProvider
    {
        void DeleteAddress(int addressID);

        Dictionary<int, string> GetAddressNames();
    }
}
